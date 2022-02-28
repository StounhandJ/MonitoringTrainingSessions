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
    public List<Group> Group
    {
        get
        {
            return UserGroups.getAll(new Dictionary<string, object?>() { { "user_id", id } })
                .ConvertAll(userGroups => userGroups.Group);
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
        this.password = password;
    }

    public User(string login, string password, string fio, Role role)
    {
        this.Role = role;
        this.FIO = fio;

        this.login = login;
        this.password = password;
    }

    public User()
    {
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