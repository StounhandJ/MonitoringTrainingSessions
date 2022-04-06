using System;

namespace MonitoringTrainingSessions.Lib.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class Additional : Attribute
{
}