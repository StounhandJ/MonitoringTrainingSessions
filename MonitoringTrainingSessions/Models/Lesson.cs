using System;
using System.Collections.Generic;
using System.Linq;
using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Lesson : Model<Lesson>
{
    protected override string tableName
    {
        get => "lesson";
    }

    private int id { get; set; }

    private int schedule_id { get; set; }

    public string topic { get; set; }

    public string task { get; set; }

    public string discord_id { get; set; }

    public DateTime date { get; set; }

    [Additional]
    public int Id
    {
        get => id;
    }

    [Additional]
    public Schedule Schedule
    {
        get => Schedule.getById(schedule_id);
        set
        {
            if (value.exist())
            {
                schedule_id = value.Id;
            }
        }
    }

    public static Lesson getOrCreate(Schedule schedule, DateTime date)
    {
        Lesson lesson = Lesson.select(new Dictionary<string, object?>()
        {
            { "schedule_id", schedule.Id },
            { "date", date.Date }
        });
        if (lesson.exist())
        {
            return lesson;
        }

        lesson.date = date;
        lesson.Schedule = schedule;
        lesson.topic = "-";
        lesson.task = "-";
        lesson.generateDisordId();
        return lesson;
    }

    public void generateDisordId()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        this.discord_id = new string(Enumerable.Repeat(chars, 25)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}