using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class UserGroups : Model<UserGroups>
{
    protected override string tableName
    {
        get => "user_groups";
    }

    private int user_id { get; set; }

    private int group_id { get; set; }
    
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
}