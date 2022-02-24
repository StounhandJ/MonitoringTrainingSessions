using System.Collections.Generic;
using MonitoringTrainingSessions.Lib;
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

    public int Id
    {
        get => id;
    }

    public Group Group
    {
        get => Group.getById(group_id);
    }

    public Role Role
    {
        get => Role.getById(role_id);
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