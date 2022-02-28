using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands.DelegateCommand;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class CahngeUserViewModel : BaseViewModel
{
    public CahngeUserViewModel(User user, Page page, MainViewModel dataContext)
    {
        DataContext = dataContext;
        User = user;
        _exitPage = page;
        Sessions = new ObservableCollection<Session>(Session.getAll());
        Groups = new ObservableCollection<Group>(Group.getAll());
        ExitCommand = new DelegateCommand((o) => { Exit(); });
        SaveCommand = new DelegateCommand((o) =>
        {
            User.save();
            Exit();
        });
        
        DeleteCommand = new DelegateCommand((o) =>
        {
            User.delete();
            Exit();
        });
    }

    public CahngeUserViewModel()
    {
    }

    private Page _exitPage;
    
    private User _user;

    public User User
    {
        get => _user;
        set
        {
            _user = value;
            this.OnPropertyChanged(nameof(User));
        }
    }
    
    private Role _role;

    public Role Role
    {
        get => _role;
        set
        {
            _role = value;
            this.OnPropertyChanged(nameof(Role));
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

    public ICommand ExitCommand { get; set; }
    
    public ICommand SaveCommand { get; set; }
    
    public ICommand DeleteCommand { get; set; }

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

    public void Exit()
    {
        DataContext.Content = _exitPage;
    }
}