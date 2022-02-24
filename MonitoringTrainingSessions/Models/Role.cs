using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Role: Model<Role>
{
    public const int TEACHER = 1;
    public const int STUDENT = 2;
    
    protected override string tableName { get=>"roles"; }

    private int id { get; set; }
    
    public string name { get; set; }

    public int Id
    {
        get => id;
    }
}