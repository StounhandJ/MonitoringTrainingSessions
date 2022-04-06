using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class ScheduleViewModel : BaseViewModel
{
    public ScheduleViewModel()
    {
        SaveCommand = new SaveGroupsScheduleCommand();
    }

    public ICommand SaveCommand { get; set; }

    private ObservableCollection<GroupSchedule> _groupsSchedule = new ObservableCollection<GroupSchedule>();

    public ObservableCollection<GroupSchedule> GroupsSchedule
    {
        get => _groupsSchedule;
        set
        {
            _groupsSchedule = value;
            this.OnPropertyChanged(nameof(GroupsSchedule));
        }
    }

    private MainViewModel _dataContext;

    public MainViewModel DataContext
    {
        get => _dataContext;
        set
        {
            _dataContext = value;
            Sessions = value.Sessions;
            SelectedDay = 1;
            this.OnPropertyChanged(nameof(DataContext));
        }
    }

    private ObservableCollection<Session> _sessions;

    public ObservableCollection<Session> Sessions
    {
        get => _sessions;
        set
        {
            value.Insert(0, new Session());
            _sessions = value;
            SelectedDay = 1;
            this.OnPropertyChanged(nameof(Sessions));
        }
    }

    private int _selectedDay;

    public int SelectedDay
    {
        get => _selectedDay;
        set
        {
            _selectedDay = value;

            GroupsSchedule.Clear();
            foreach (var group in DataContext.Groups)
            {
                // Task.Run(async () =>
                // {
                    GroupsSchedule.Add(new GroupSchedule(group, Schedule.getByGroupDay(group, SelectedDay),
                    new List<Session>(Sessions), SelectedDay));
                // });
                
            }

            this.OnPropertyChanged(nameof(SelectedDay));
        }
    }
}