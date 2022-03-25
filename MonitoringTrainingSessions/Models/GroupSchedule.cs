using System.Collections.Generic;

namespace MonitoringTrainingSessions.Models;

public class GroupSchedule
{
    public Group group { get; set; }
    public List<Schedule> schedules { get; set; }
    
    public List<Session> sessions { get; set; }
    
    public int day { get; set; }
    

    public GroupSchedule(Group group, List<Schedule> schedules, List<Session> sessions, int day)
    {
        this.@group = group;
        this.schedules = schedules;
        this.sessions = sessions;
        this.day = day;
    }

    public Schedule schedulePairOne
    {
        get
        {
            Schedule? schedule = schedules.Find(s => s.number_pair == 1);
            if (schedule==null)
            {
                schedule = new Schedule(group, new Session(), day, 1);
            }
            return schedule;
        }
        set
        {
            var s = schedules.Find(s => s.number_pair == 1);
            s = value;
        }
    }

    public Schedule schedulePairTwo
    {
        get
        {
            Schedule? schedule = schedules.Find(s => s.number_pair == 2);
            if (schedule==null)
            {
                schedule = new Schedule(group, new Session(), day, 2);
            }
            return schedule;
        }
        set
        {
            var s = schedules.Find(s => s.number_pair == 2);
            s = value;
        }
    }

    public Schedule schedulePairThree
    {
        get
        {
            Schedule? schedule = schedules.Find(s => s.number_pair == 3);
            if (schedule==null)
            {
                schedule = new Schedule(group, new Session(), day, 3);
            }
            return schedule;
        }
        set
        {
            var s = schedules.Find(s => s.number_pair == 3);
            s = value;
        }
    }

    public Schedule schedulePairFour
    {
        get
        {
            Schedule? schedule = schedules.Find(s => s.number_pair == 4);
            if (schedule==null)
            {
                schedule = new Schedule(group, new Session(), day, 4);
            }
            return schedule;
        }
        set
        {
            var s = schedules.Find(s => s.number_pair == 4);
            s = value;
        }
    }

    public Schedule schedulePairFive
    {
        get
        {
            Schedule? schedule = schedules.Find(s => s.number_pair == 5);
            if (schedule==null)
            {
                schedule = new Schedule(group, new Session(), day, 5);
            }
            return schedule;
        }
        set
        {
            var s = schedules.Find(s => s.number_pair == 5);
            s = value;
        }
    }
}