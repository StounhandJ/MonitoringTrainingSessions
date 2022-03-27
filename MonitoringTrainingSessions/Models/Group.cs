using System.Collections.Generic;
using System.Linq;
using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Group : Model<Group>
{
    protected override string tableName { get=>"groups"; }

    private int id { get; set; }
    
    public string name { get; set; }

    [Additional]
    public int Id
    {
        get => id;
    }
    
    [Additional]
    public List<User> Users
    {
        get
        {
            return UserGroups.selectAll(new Dictionary<string, object?>() { { "group_id", id } })
                .ConvertAll(userGroups => userGroups.User);
        }
        set
        {
            var currentUsers = Users; 
            foreach (var user in currentUsers)
            {
                if (!value.Any(u => u.Equals(user)))
                {
                    UserGroups.getByUserAndGroup(user, this).delete();
                }
            }

            foreach (var user in value)
            {
                if (!currentUsers.Any(u => u.Equals(user)))
                {
                    (new UserGroups(user, this)).save();
                }
            }
        }
    }
    
    public new static bool Equals(object objA, object objB)
    {
        if (objA.GetType() == objB.GetType() && objB.GetType() == typeof(Group))
            return ((Group)objA).id == ((Group)objB).id;
        return false;
    }
    
    public override bool Equals(object? objA)
    {
        if (objA == null)
            return false;
        return Group.Equals(this, objA);
    }
    
    public override string ToString()
    {
        return this.name;
    }
}