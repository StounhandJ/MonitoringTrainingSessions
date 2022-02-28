using System.Collections.Generic;
using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Schedule: Model<Schedule>
{
    protected override string tableName { get=>"schedule"; }

    private int id { get; set; }
    
    private int session_id { get; set; }
    
    private int group_id { get; set; }
    
    public int number_day_week { get; set; }

    [Additional]
    public int Id
    {
        get => id;
    }
    
    [Additional]
    public Group Group
    {
        get => Group.getById(group_id);
        set
        {
            if (value.exist())
            {
                group_id = value.Id;
            }
        }
    }
    
    [Additional]
    public Session Session
    {
        get => Session.getById(session_id);
        set
        {
            if (value.exist())
            {
                session_id = value.Id;
            }
        }
    }
    
    public static Schedule getByGroupDay(Group group, int day)
    {
        return Schedule.select(new Dictionary<string, object?>()
        {
            { "group_id", group.Id },
            { "number_day_week", day}
        });
    }
}