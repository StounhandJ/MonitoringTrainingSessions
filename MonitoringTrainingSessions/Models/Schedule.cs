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
    private TimeSchedule? _timeSchedule;
    
    [Additional]
    public TimeSchedule TimeSchedule
    {
        get
        {
            if (_timeSchedule != null)
                return _timeSchedule;
            _timeSchedule = TimeSchedule.getById(number_pair_id);
            return _timeSchedule;
        }
        set
        {
            if (value.exist())
            {
                number_pair_id = value.Id;
                _timeSchedule = value;
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
        get
        {
            return session_id==-1? new Session() : Session.getById(session_id);
        }
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
    
    public Schedule(Group Group, Session Session, int numberDayWeek, TimeSchedule timeSchedule)
    {
        this.Group = Group;
        this.Session = Session;
        number_day_week = numberDayWeek;
        TimeSchedule = timeSchedule;
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