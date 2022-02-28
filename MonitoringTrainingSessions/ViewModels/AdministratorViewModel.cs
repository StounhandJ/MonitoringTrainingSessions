using System.Collections.ObjectModel;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands.DelegateCommand;
using MonitoringTrainingSessions.Models;
using MonitoringTrainingSessions.Pages;

namespace MonitoringTrainingSessions.ViewModels;

public class AdministratorViewModel : BaseViewModel
{
    public AdministratorViewModel()
    {
        Users = new ObservableCollection<User>(User.getAll());
        Sessions = new ObservableCollection<Session>(Session.getAll());
        Groups = new ObservableCollection<Group>(Group.getAll());
        DoubleClickCommand = new DelegateCommand((o) =>
        {
            DataContext.Content = new CahngeUserPage((User)o, new AdministratorPage() { DataContext = this }, DataContext)
            {
                DataContext = new CahngeUserViewModel((User)o, new AdministratorPage() { DataContext = this }, DataContext)
            };
        });
    }

    private ObservableCollection<User> _users;

    public ObservableCollection<User> Users
    {
        get => _users;
        set
        {
            _users = value;
            this.OnPropertyChanged(nameof(Users));
        }
    }

    private User _selectedUser;

    public User SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            this.OnPropertyChanged(nameof(SelectedUser));
        }
    }

    private ObservableCollection<Session> _sessions;

    public ObservableCollection<Session> Sessions
    {
        get => _sessions;
        set
        {
            _sessions = value;
            this.OnPropertyChanged(nameof(Sessions));
        }
    }

    private ObservableCollection<Group> _groups;

    public ObservableCollection<Group> Groups
    {
        get => _groups;
        set
        {
            _groups = value;
            this.OnPropertyChanged(nameof(Groups));
        }
    }

    public ICommand DoubleClickCommand { get; set; }

    private MainViewModel _dataContext;

    public MainViewModel DataContext
    {
        get => _dataContext;
        set
        {
            _dataContext = value;
            this.OnPropertyChanged(nameof(DataContext));
        }
    }
}