using MonitoringTrainingSessions.Lib;

namespace MonitoringTrainingSessions.Models;

public class User : Model
{
    protected override string tableName { get=>"users"; }

    protected int id { get; set; }
    public int role_id { get; set; }
    public int group_id { get; set; }

    public string name { get; set; }
    public string surname { get; set; }
    public string patronymic { get; set; }
    public string login { get; set; }
    public string password { get; set; }

    public int Id
    {
        get => id;
    }
    
    public Group Group
    {
        get
        {
            Group group = new Group();
            group.getById(group_id);
            return group;
        }
    }
}