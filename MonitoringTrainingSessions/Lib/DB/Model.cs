using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonitoringTrainingSessions.Lib.Attributes;

namespace MonitoringTrainingSessions.Lib.DB;

abstract public class Model<T> : IModel
    where T : IModel
{
    private static DBConnector _dbConnector = new DBConnector();

    protected Model()
    {
        this.setNullIndex();
    }

    protected abstract string tableName { get; }

    public static List<T> getAll(Dictionary<string, object?>? data = null)
    {
        T? model = Model<T>.constrct();

        return model.@selectAll().ConvertAll(input => (T)input);
    }

    public static T getById(int id)
    {
        T? model = Model<T>.constrct();

        model.@select(new Dictionary<string, object?>() { { "id", id } });

        return model;
    }

    public static T select(Dictionary<string, object?>? data)
    {
        T? model = Model<T>.constrct();

        model.@select(data);

        return model;
    }

    private static T constrct()
    {
        Type type = typeof(T);
        ConstructorInfo? constructorInfoObj = type.GetConstructor(new Type[] { });

        T? model = (T)constructorInfoObj?.Invoke(new object?[] { })!;
        return model;
    }

    void IModel.select(Dictionary<string, object?>? data)
    {
        string sql = string.Format("select * from \"{0}\" ", this.tableName);

        if (data != null)
        {
            sql += string.Format("where {0} ", generateParametrsWhere(data));
        }

        var result = _dbConnector.execute(sql, data);

        if (result.Count != 0)
        {
            setPropertiesValue(result[0]);
        }
    }

    public List<object> selectAll(Dictionary<string, object?>? data)
    {
        string sql = string.Format("select * from \"{0}\" ", this.tableName);

        if (data != null)
        {
            sql += string.Format("where {0} ", generateParametrsWhere(data));
        }

        var results = _dbConnector.execute(sql, data);

        List<object> objects = new List<object>();
        foreach (var result in results)
        {
            T? model = Model<T>.constrct();
            model.setPropertiesValue(result);
            objects.Add(model);
        }

        return objects;
    }

    int IModel.count(Dictionary<string, object?>? data)
    {
        string sql = string.Format("select * from \"{0}\" ", this.tableName);

        if (data != null)
        {
            sql += string.Format("where {0} ", generateParametrsWhere(data));
        }

        var result = _dbConnector.execute(sql, data);

        return result.Count;
    }

    public void save()
    {
        List<PropertyInfo> propertes = this.getProperties();
        Dictionary<string, object?> data = new Dictionary<string, object?>();
        foreach (var property in propertes)
        {
            if (property.Name != "id")
            {
                data.Add(property.Name, property.GetValue(this));
            }
        }

        string sql;
        if (this.exist())
        {
            sql = string.Format("update \"{0}\" set {1} where id={2}",
                this.tableName,
                generateParametrs(data),
                this.GetType().GetProperty("id")!.GetValue(this)
            );
        }
        else
        {
            sql = string.Format("insert into \"{0}\" ({1}) values({2}) returning id;",
                this.tableName,
                generateNameProperty(data),
                generateValueProperty(data)
            );
        }

        var result = _dbConnector.execute(sql, data);

        if (result.Count != 0)
        {
            this.setPropertiesValue(result[0]);
        }
    }

    public void delete()
    {
        if (this.exist())
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>()
                { { "id", this.GetType().GetProperty("id")?.GetValue(this)! } };

            string sql = string.Format("delete from \"{0}\" where {1}", this.tableName, generateParametrsWhere(data));

            _dbConnector.execute(sql, data);
        }
    }

    public bool exist()
    {
        PropertyInfo? property = this.GetType().GetProperty("id");
        if (property == null)
            return false;

        object? value = property.GetValue(this);
        if (value == null)
            return false;

        return ((int)value) != -1;
    }

    public void setPropertiesValue(Dictionary<string, object> data)
    {
        this.setNullIndex();

        foreach (var parametr in data)
        {
            PropertyInfo? property = this.GetType().GetProperty(parametr.Key) ??
                                     this.GetType().GetProperty(parametr.Key,
                                         BindingFlags.NonPublic | BindingFlags.Instance);
            if (property != null)
            {
                property.SetValue(this, parametr.Value);
            }
        }
    }

    private void setNullIndex()
    {
        PropertyInfo? propertyId = this.GetType().GetProperty("id", BindingFlags.NonPublic | BindingFlags.Instance) ??
                                   this.GetType().GetProperty("id");
        if (propertyId != null)
        {
            propertyId.SetValue(this, -1);
        }
    }

    private List<PropertyInfo> getProperties()
    {
        List<PropertyInfo> properties = new List<PropertyInfo>();

        properties.AddRange(this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).ToList());
        properties.AddRange(this.GetType().GetProperties());

        properties.RemoveAll((property =>
            property.CustomAttributes.Any(attribute => attribute.AttributeType.Equals(typeof(Additional))) ||
            property.Name == "tableName"));
        return properties;
    }

    private static string generateParametrsWhere(Dictionary<string, object?> data)
    {
        string parametr = dictionaryToString("{0}=@{0} and ", data);
        return parametr.Remove(parametr.Length - 5);
    }

    private static string generateParametrs(Dictionary<string, object?> data)
    {
        string parametr = dictionaryToString("{0}=@{0}, ", data);
        return parametr.Remove(parametr.Length - 2);
    }

    private static string generateNameProperty(Dictionary<string, object?> data)
    {
        string names = dictionaryToString("{0}, ", data);
        return names.Remove(names.Length - 2);
    }

    private static string generateValueProperty(Dictionary<string, object?> data)
    {
        string value = dictionaryToString("@{0}, ", data);
        return value.Remove(value.Length - 2);
    }

    private static string dictionaryToString(string format, Dictionary<string, object?> data)
    {
        string str = "";
        foreach (var paramenr in data)
        {
            str += string.Format(format, paramenr.Key, paramenr.Value);
        }

        return str;
    }
}