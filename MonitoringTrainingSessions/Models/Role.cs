using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Role: Model<Role>
{
    public const int TEACHER = 1;
    public const int STUDENT = 2;
    public const int ADMIN = 3;
    
    protected override string tableName { get=>"roles"; }

    private int id { get; set; }
    
    public string name { get; set; }

    [Additional]
    public int Id
    {
        get => id;
    }
}