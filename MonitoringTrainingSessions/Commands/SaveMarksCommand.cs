using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.Commands;

public class SaveMarksCommand : ICommand
{
    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        var marks = (ObservableCollection<Mark>)parameter;

        foreach (var mark in marks)
        {
            if (mark.mark!=null)
            {
                mark.save();
            }
        }
    }

    public event EventHandler? CanExecuteChanged;
}