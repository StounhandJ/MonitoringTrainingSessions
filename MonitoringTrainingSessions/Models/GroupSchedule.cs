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
        List<TimeSchedule> timeSchedules = TimeSchedule.selectAll();
        for (int i = 1; i <= 5; i++)
        {
            if (!schedules.Exists(s => s.number_pair == i))
                schedules.Add(new Schedule(group, new Session(), day, timeSchedules.Find(ts => ts.number == i)));
        }
    }

    public Schedule schedulePairOne
    {
        get { return schedules.Find(s => s.number_pair == 1)!; }
        set
        {
            var s = schedules.Find(s => s.number_pair == 1);
            s = value;
        }
    }

    public Schedule schedulePairTwo
    {
        get { return schedules.Find(s => s.number_pair == 2)!; }
        set
        {
            var s = schedules.Find(s => s.number_pair == 2);
            s = value;
        }
    }

    public Schedule schedulePairThree
    {
        get { return schedules.Find(s => s.number_pair == 3)!; }
        set
        {
            var s = schedules.Find(s => s.number_pair == 3);
            s = value;
        }
    }

    public Schedule schedulePairFour
    {
        get { return schedules.Find(s => s.number_pair == 4)!; }
        set
        {
            var s = schedules.Find(s => s.number_pair == 4);
            s = value;
        }
    }

    public Schedule schedulePairFive
    {
        get { return schedules.Find(s => s.number_pair == 5)!; }
        set
        {
            var s = schedules.Find(s => s.number_pair == 5);
            s = value;
        }
    }
}