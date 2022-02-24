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
}