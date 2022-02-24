using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Schedule: Model<Schedule>
{
    protected override string tableName { get=>"schedule"; }

    private int id { get; set; }
    
    private int session_id { get; set; }
    
    private int group_id { get; set; }
    
    public int number_day_week { get; set; }

    public int Id
    {
        get => id;
    }
    
    public Group Group
    {
        get => Group.getById(group_id);
    }
    
    public Session Session
    {
        get => Session.getById(session_id);
    }
}