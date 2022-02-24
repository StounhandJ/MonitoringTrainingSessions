using System;
using System.Collections.Generic;
using Npgsql;

namespace MonitoringTrainingSessions.Lib.DB;

public class DBConnector
{
    private string _server;
    private string _databaseName;
    private string _userName;
    private string _password;

    public DBConnector()
    {
        if (DBConnector.Connection == null)
        {
            throw new Exception("DataBase no conected");
        }
    }

    public List<Dictionary<string, object>> execute(string sql, Dictionary<string, object?>? data = null)
    {
        List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
        var cmd = new NpgsqlCommand(sql, DBConnector.Connection);
        if (data != null)
        {
            foreach (var parametr in data)
            {
                cmd.Parameters.AddWithValue(parametr.Key, parametr.Value);
            }
        }

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            bool start = true;
            int number = 0;
            while (start)
            {
                try
                {
                    row.Add(reader.GetName(number), reader.GetValue(number));
                    number++;
                }
                catch (Exception)
                {
                    start = false;
                }
            }

            result.Add(row);
        }

        reader.Close();
        return result;
    }

    private static NpgsqlConnection? Connection { get; set; }

    public static bool connect(string server, string databaseName, string userName, string password)
    {
        if (String.IsNullOrEmpty(databaseName) || String.IsNullOrEmpty(server) || String.IsNullOrEmpty(userName))
            return false;
        string connstring = string.Format("Host={0};Username={1};Password={2};Database={3}", server,
            userName,
            password, databaseName);
        Connection = new NpgsqlConnection(connstring);
        Connection.Open();

        return true;
    }

    private void Close()
    {
        Connection?.Close();
    }
}