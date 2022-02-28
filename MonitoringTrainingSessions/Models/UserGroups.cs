using System.Collections.Generic;
using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

[ManyToMany]
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

    [Additional]
    public User User
    {
        get => User.getById(user_id);
        set
        {
            if (value.exist())
            {
                user_id = value.Id;
            }
        }
    }

    public UserGroups(User user, Group group)
    {
        this.User = user;
        this.Group = group;
    }

    public UserGroups()
    {
    }

    public static UserGroups getByUserAndGroup(User user, Group group)
    {
        return UserGroups.select(new Dictionary<string, object?>()
        {
            { "user_id", user.Id },
            { "group_id", group.Id }
        });
    }

    public void delete()
    {
        this.delete(new Dictionary<string, object?>()
        {
            { "user_id", user_id },
            { "group_id", group_id }
        });
    }
}