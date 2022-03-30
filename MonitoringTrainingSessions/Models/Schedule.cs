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
    
    private int number_pair_id { get; set; }
    
    [Additional]
    public TimeSchedule TimeSchedule
    {
        get => TimeSchedule.getById(number_pair_id);
        set
        {
            if (value.exist())
            {
                number_pair_id = value.Id;
            }
        }
    }

    [Additional]
    public int number_pair
    {
        get => TimeSchedule.number;
        set
        {
            TimeSchedule = TimeSchedule.getByNumber(value);
        } 
    }

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
            // if (value.exist())
            // {
                session_id = value.Id;
            // }
        }
    }

    public Schedule(Group Group, Session Session, int numberDayWeek, int numberPair)
    {
        this.Group = Group;
        this.Session = Session;
        number_day_week = numberDayWeek;
        number_pair = numberPair;
    }

    public Schedule()
    {
    }

    public static Schedule getByGroupDay(Group group, int day, TimeSchedule timeSchedule)
    {
        return Schedule.select(new Dictionary<string, object?>()
        {
            { "group_id", group.Id },
            { "number_day_week", day},
            { "number_pair_id", timeSchedule.Id}
        });
    }
    
    public static List<Schedule> getByGroupDay(Group group, int day)
    {
        return Schedule.selectAll(new Dictionary<string, object?>()
        {
            { "group_id", group.Id },
            { "number_day_week", day}
        });
    }
}