using System.Collections.ObjectModel;
using System.Windows.Controls;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class MainViewModel : BaseViewModel
{
    private User _user;

    public User User
    {
        get => _user;
        set
        {
            _user = value;
            checkUser();
            this.OnPropertyChanged(nameof(User));
        }
    }

    private Page _content { get; set; }

    public Page Content
    {
        get => _content;
        set
        {
            _content = value;
            this.OnPropertyChanged(nameof(Content));
        }
    }

    private ObservableCollection<Role> _roles;

    public ObservableCollection<Role> Roles
    {
        get => _roles;
        set
        {
            _roles = value;
            this.OnPropertyChanged(nameof(Roles));
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

    public MainViewModel()
    {
        Roles =  new ObservableCollection<Role>(Role.getAll());
        Groups =  new ObservableCollection<Group>(Group.getAll());
    }

    public void checkUser()
    {
    }
}