using System;
using System.Collections.Generic;
using System.Linq;
using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Lib.Attributes;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

[ManyToMany]
public class User : Model<User>
{
    protected override string tableName
    {
        get => "users";
    }

    private int id { get; set; }
    private int role_id { get; set; }

    public string name { get; set; }
    public string surname { get; set; }
    public string patronymic { get; set; }
    public string login { get; set; }

    private string _password;

    public string password
    {
        private get => _password;
        set
        {
            if (_password == null)
            {
                _password = value;
            }
            else
            {
                _password = Encryption.CreateMD5(value);
            }
        }
    }

    [Additional]
    public int Id
    {
        get => id;
    }

    [Additional]
    public string FIO
    {
        get => string.Format("{0} {1} {2}", this.surname, this.name, this.patronymic);
        set
        {
            List<string> data = new List<string>(value.Split(' '));
            if (data.Count >= 1)
            {
                this.surname = data[0];
            }

            if (data.Count >= 2)
            {
                this.name = data[1];
            }

            if (data.Count >= 3)
            {
                this.patronymic = data[2];
            }
        }
    }

    [Additional]
    public List<Group> Groups
    {
        get
        {
            return UserGroups.selectAll(new Dictionary<string, object?>() { { "user_id", id } })
                .ConvertAll(userGroups => userGroups.Group);
        }
        set
        {
            var currentGroups = Groups;
            foreach (var group in currentGroups)
            {
                if (!value.Any(g => g.Equals(group)))
                {
                    UserGroups.getByUserAndGroup(this, group).delete();
                }
            }

            foreach (var group in value)
            {
                if (!currentGroups.Any(g => g.Equals(group)))
                {
                    (new UserGroups(this, group)).save();
                }
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

    public User(string login, string password, string surname, string name, string patronymic, Role role)
    {
        this.Role = role;
        this.name = name;
        this.surname = surname;
        this.patronymic = patronymic;

        this.login = login;
        this._password = password;
    }

    public User(string login, string password, string fio, Role role)
    {
        this.Role = role;
        this.FIO = fio;

        this.login = login;
        this._password = password;
    }

    public User()
    {
    }

    public Schedule CurrentSchedule(Group? group = null)
    {
        TimeSchedule? timeSchedule = TimeSchedule.getCurrentTimeSchedule();
        if (timeSchedule == null || Groups.Count == 0)
            return new Schedule();

        return Schedule.getByGroupDay(group ?? Groups.First(), (int)DateTime.Now.DayOfWeek, timeSchedule);
    }

    public static User getByLoginPassword(string login, string password)
    {
        return User.select(new Dictionary<string, object?>()
        {
            { "login", login },
            { "password", Encryption.CreateMD5(password) }
        });
    }

    public static User getByLogin(string login)
    {
        return User.select(new Dictionary<string, object?>()
        {
            { "login", login }
        });
    }
}