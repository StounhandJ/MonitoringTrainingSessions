﻿using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Group : Model<Group>
{
    protected override string tableName { get=>"groups"; }

    private int id { get; set; }
    
    public string name { get; set; }

    public int Id
    {
        get => id;
    }
}