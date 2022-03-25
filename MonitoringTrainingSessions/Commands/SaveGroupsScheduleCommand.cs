using System;
using System.Collections.Generic;
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
        var groupsSchedule = (List<GroupSchedule>)parameter;

        foreach (var groupSchedule in groupsSchedule)
        {
            foreach (var schedule in groupSchedule.schedules)
            {
                schedule.save();
            }
        }
    }

    public event EventHandler? CanExecuteChanged;
}