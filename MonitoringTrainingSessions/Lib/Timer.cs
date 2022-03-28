using System;
using System.Windows.Threading;

namespace MonitoringTrainingSessions.Lib;

public class Timer
{
    public static void start(Action act, int second = 5)
    {
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = new TimeSpan(0, 0, second);
        timer.Tick += (o, args) =>
        {
            act.Invoke();
        };
        timer.Start();
    }
}