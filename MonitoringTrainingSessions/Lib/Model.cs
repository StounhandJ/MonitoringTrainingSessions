using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonitoringTrainingSessions.Lib;

abstract public class Model
{
    private DB db = new DB();

    protected Model()
    {
        this.setNullIndex();
    }

    protected abstract string tableName { get; }
    
    
    public void getById(int id)
    {
        this.@select(new Dictionary<string, object?>(){{"id", id}});
    }

    public void select(Dictionary<string, object?>? data = null)
    {
        string sql = string.Format("select * from \"{0}\" ", this.tableName);

        if (data != null)
        {
            sql += string.Format("where {0} ", this.generateParametrs(data));
        }

        var result = db.execute(sql, data);

        if (result.Count != 0)
        {
            this.setPropertiesValue(result[0]);
        }
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
                this.generateParametrs(data),
                this.GetType().GetProperty("id")!.GetValue(this)
            );
        }
        else
        {
            sql = string.Format("insert into \"{0}\" ({1}) values({2}) returning id;",
                this.tableName,
                this.generateNameProperty(data),
                this.generateValueProperty(data)
            );
        }

        var result = db.execute(sql, data);

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

            string sql = string.Format("delete from \"{0}\" where {1}", this.tableName, this.generateParametrs(data));

            db.execute(sql, data);
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

    private void setPropertiesValue(Dictionary<string, object> data)
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

        properties.RemoveAll((property => !property.CanWrite));

        return properties;
    }

    private string generateParametrs(Dictionary<string, object?> data)
    {
        return this.dictionaryToString("{0}=@{0} ", data);
    }

    private string generateNameProperty(Dictionary<string, object?> data)
    {
        string names = this.dictionaryToString("{0}, ", data);
        return names.Remove(names.Length - 2);
    }

    private string generateValueProperty(Dictionary<string, object?> data)
    {
        string value = this.dictionaryToString("@{0}, ", data);
        return value.Remove(value.Length - 2);
    }

    private string dictionaryToString(string format, Dictionary<string, object?> data)
    {
        string str = "";
        foreach (var paramenr in data)
        {
            str += string.Format(format, paramenr.Key, paramenr.Value);
        }

        return str;
    }
}