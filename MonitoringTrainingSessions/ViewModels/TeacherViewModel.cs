using System.Collections.Generic;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class TeacherViewModel : BaseViewModel
{
    private List<Group> _groups;

    public List<Group> Groups
    {
        get => _groups;
        set
        {
            _groups = value;
            this.OnPropertyChanged(nameof(Groups));
        }
    }

    private MainViewModel _dataContext;

    public MainViewModel DataContext
    {
        get => _dataContext;
        set
        {
            _dataContext = value;
            Groups = _dataContext.User?.Groups;
            this.OnPropertyChanged(nameof(DataContext));
        }
    }
}