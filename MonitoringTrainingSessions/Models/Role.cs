using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Role : Model<Role>
{
    public const int TEACHER = 1;
    public const int STUDENT = 2;
    public const int ADMIN = 3;

    protected override string tableName
    {
        get => "roles";
    }

    private int id { get; set; }

    public string name { get; set; }

    [Additional]
    public int Id
    {
        get => id;
    }

    public static bool operator ==(Role f1, Role f2)
    {
        return Role.Equals(f1, f2);
    }

    public static bool operator !=(Role f1, Role f2)
    {
        return !Role.Equals(f1, f2);
    }

    public new static bool Equals(object objA, object objB)
    {
        if (objA.GetType() == objB.GetType() && objB.GetType() == typeof(Role))
            return ((Role)objA).id == ((Role)objB).id;
        return false;
    }
    
    public override bool Equals(object? objA)
    {
        if (objA == null)
            return false;
        return Role.Equals(this, objA);
    }
}