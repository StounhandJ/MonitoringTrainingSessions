using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Group : Model<Group>
{
    protected override string tableName { get=>"groups"; }

    private int id { get; set; }
    
    public string name { get; set; }

    [Additional]
    public int Id
    {
        get => id;
    }
    public new static bool Equals(object objA, object objB)
    {
        if (objA.GetType() == objB.GetType() && objB.GetType() == typeof(Group))
            return ((Group)objA).id == ((Group)objB).id;
        return false;
    }
    
    public override bool Equals(object? objA)
    {
        if (objA == null)
            return false;
        return Group.Equals(this, objA);
    }
    
    public override string ToString()
    {
        return this.name;
    }
}