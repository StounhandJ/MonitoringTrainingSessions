using System;

namespace MonitoringTrainingSessions.Lib.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class ManyToMany : Attribute
{
    public Type model;

    public string yourField;

    public string foreignField;

    public string tableName;

    public ManyToMany(Type model, string yourField, string foreignField, string tableName)
    {
        this.model = model;
        this.yourField = yourField;
        this.foreignField = foreignField;
        this.tableName = tableName;
    }
}