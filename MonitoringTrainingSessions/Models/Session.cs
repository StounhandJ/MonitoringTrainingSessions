using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Session: Model<Session>
{
    protected override string tableName { get=>"sessions"; }

    private int id { get; set; }
    
    public string name { get; set; }

    public int Id
    {
        get => id;
    }
}