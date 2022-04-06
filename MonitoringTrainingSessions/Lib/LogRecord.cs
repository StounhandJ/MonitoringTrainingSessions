using System;
using System.IO;
using MonitoringTrainingSessions.Lib;

namespace MonitoringTrainingSessions.Lib;

public class LogViewer
{
    public string pathLog { get; private set; }

    private string nameDirectory = "log";

    private DateTime startDate;

    public LogViewer()
    {
        string path = Environment.CurrentDirectory + "\\";
        if (!Directory.Exists(path + nameDirectory))
        {
            Directory.CreateDirectory(path + nameDirectory);
        }

        this.startDate = DateTime.Now;
        this.pathLog = path + nameDirectory + "\\" + this.startDate.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss'_'ffff") +
                       ".txt";
        File.Create(this.pathLog).Close();
    }

    public void log(string logs, Status status)
    {
        using (StreamWriter writer = new StreamWriter(this.pathLog, append: true))
        {
            writer.AutoFlush = true;
            writer.WriteLine($"{this.date()}|  {this.decodingStatus(status)}| {logs}");
        }
    }

    private string date()
    {
        var test = DateTime.Now - this.startDate;
        var d = new DateTime(test.Ticks);
        return d.ToString("mm'.'ss'.'fffffff");
    }

    private string decodingStatus(Status status)
    {
        switch (status)
        {
            case Status.Info: return "INFO";
            case Status.Ok: return "OKAY";
            case Status.Error: return "ERROR";
        }

        return "";
    }
}