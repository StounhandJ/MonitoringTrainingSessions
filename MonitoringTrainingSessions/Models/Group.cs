using MonitoringTrainingSessions.Lib;

namespace MonitoringTrainingSessions.Models;

public class Group : Model
{
    protected override string tableName { get=>"groups"; }

    protected int id { get; set; }
    
    public string name { get; set; }

    public int Id
    {
        get => id;
    }
}