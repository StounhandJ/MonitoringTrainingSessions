using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.Commands;

public class SaveGroupsScheduleCommand : ICommand
{
    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        var groupsSchedule = (ObservableCollection<GroupSchedule>)parameter;

        foreach (var groupSchedule in groupsSchedule)
        {
            foreach (var schedule in groupSchedule.schedules)
            {
                if (schedule.Session.exist())
                {
                    schedule.save();
                }
                else if (schedule.exist())
                {
                    schedule.delete();
                }
            }
        }
    }

    public event EventHandler? CanExecuteChanged;
}