using System;
using System.Collections.Generic;
using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class TimeSchedule : Model<TimeSchedule>
{
    protected override string tableName
    {
        get => "time_schedule";
    }

    private int id { get; set; }

    public int number { get; set; }

    public TimeSpan start_time { get; set; }

    public TimeSpan end_time { get; set; }

    [Additional]
    public int Id
    {
        get => id;
    }

    public static TimeSchedule getByNumber(int number)
    {
        return TimeSchedule.select(new Dictionary<string, object?>()
        {
            { "number", number }
        });
    }

    public static List<TimeSchedule> selectAll(Dictionary<string, object?>? data = null)
    {
        return TimeSchedule.selectAll(data, after_where: "ORDER BY number ASC");
    }
}