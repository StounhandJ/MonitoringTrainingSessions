using System;
using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions.Models;

public class Mark: Model<Mark>
{
    protected override string tableName { get=>"marks"; }

    private int id { get; set; }
    
    private int session_id { get; set; }
    
    private int who_put_user { get; set; }
    
    private int who_was_put_user { get; set; }
    
    public int mark { get; set; }
    
    public DateTime date { get; set; }

    public int Id
    {
        get => id;
    }
    
    public User whoPutUser
    {
        get => User.getById(who_put_user);
    }
    
    public User whoWasPutUser
    {
        get => User.getById(who_was_put_user);
    }
    
    public Session Session
    {
        get => Session.getById(session_id);
    }
}