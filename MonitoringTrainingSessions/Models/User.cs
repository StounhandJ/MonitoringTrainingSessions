using System.Collections.Generic;
using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class User : Model<User>
{
    protected override string tableName
    {
        get => "users";
    }

    private int id { get; set; }
    private int role_id { get; set; }
    private int group_id { get; set; }

    public string name { get; set; }
    public string surname { get; set; }
    public string patronymic { get; set; }
    public string login { get; set; }

    private string _password;

    public string password
    {
        private get => _password;
        set => _password = Encryption.CreateMD5(value);
    }

    [Additional]
    public int Id
    {
        get => id;
    }

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
    public Role Role
    {
        get => Role.getById(role_id);
        set
        {
            if (value.exist())
            {
                role_id = value.Id;
            }
        }
    }

    public User(string login, string password, string surname, string name, string patronymic, Role role, Group group)
    {
        
        this.Role = role;
        this.Group = group;
        this.name = name;
        this.surname = surname;
        this.patronymic = patronymic;
        
        this.login = login;
        this.password = password;
    }

    public static User getByLoginPassword(string login, string password)
    {
        return User.select(new Dictionary<string, object?>()
        {
            { "login", login },
            { "password", Encryption.CreateMD5(password) }
        });
    }
}