using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Session: Model<Session>
{
    protected override string tableName { get=>"sessions"; }

    private int id { get; set; }
    
    public string name { get; set; }

    [Additional]
    public int Id
    {
        get => id;
    }
    
    public new static bool Equals(object objA, object objB)
    {
        if (objA.GetType() == objB.GetType() && objB.GetType() == typeof(Session))
            return ((Session)objA).id == ((Session)objB).id;
        return false;
    }
    
    public override bool Equals(object? objA)
    {
        if (objA == null)
            return false;
        return Session.Equals(this, objA);
    }
    
    public override string ToString()
    {
        return this.name;
    }
}