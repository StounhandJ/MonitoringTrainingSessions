using System;
using System.Collections.Generic;
using MonitoringTrainingSessions.Lib.Attributes;
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

    [Additional]
    public int Id
    {
        get => id;
    }
    
    [Additional]
    public User whoPutUser
    {
        get => User.getById(who_put_user);
        set
        {
            if (value.exist())
            {
                who_put_user = value.Id;
            }
        }
    }
    
    [Additional]
    public User whoWasPutUser
    {
        get => User.getById(who_was_put_user);
        set
        {
            if (value.exist())
            {
                who_was_put_user = value.Id;
            }
        }
    }
    
    [Additional]
    public Session Session
    {
        get => Session.getById(session_id);
        set
        {
            if (value.exist())
            {
                session_id = value.Id;
            }
        }
    }
    
    public static Mark getBySessionUserDate(Session session, User whoWasUser, DateTime date)
    {
        return Mark.select(new Dictionary<string, object?>()
        {
            { "session_id", session.Id },
            { "who_was_put_user", whoWasUser.Id},
            { "date", date}
        });
    }
}