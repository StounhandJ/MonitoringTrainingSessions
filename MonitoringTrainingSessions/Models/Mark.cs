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
    
    public int? mark { get; set; }
    
    public DateTime date { get; set; }

    [Additional] private List<int?> _marks = new List<int?>() { 2, 3, 4, 5 };
    
    [Additional]
    public List<int?> marks { get=>_marks; set=>_marks=value; }
    
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

    public Mark()
    {
    }

    public Mark(Session session, User whoPutUser, User whoWasPutUser, int? mark, DateTime date)
    {
        Session = session;
        this.whoPutUser = whoPutUser;
        this.whoWasPutUser = whoWasPutUser;
        this.mark = mark;
        this.date = date;
    }

    public static Mark getBySessionUserDate(Session session, User whoWasUser, DateTime date)
    {
        return Mark.select(new Dictionary<string, object?>()
        {
            { "session_id", session.Id },
            { "who_was_put_user", whoWasUser.Id},
            { "date", date.Date}
        });
    }
    
    public static List<Mark> getByGroupSessionDate(Group group, Session session, DateTime date)
    {
        return Mark.selectAll(new Dictionary<string, object?>()
        {
            { "session_id", session.Id },
            { "group_id", group.Id},
            { "date", date.Date}
        }, "JOIN user_groups ON user_groups.user_id = marks.who_was_put_user");
    }
    
    public static List<Mark> getByUserDate(User who_was_put_user, DateTime date)
    {
        return Mark.selectAll(new Dictionary<string, object?>()
        {
            { "who_was_put_user", who_was_put_user.Id },
            { "date", date.Date}
        });
    }
}