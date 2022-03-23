using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands;
using MonitoringTrainingSessions.Models;
using MonitoringTrainingSessions.Pages;

namespace MonitoringTrainingSessions.ViewModels;

public class MainViewModel : BaseViewModel
{
    private User? _user;

    public User? User
    {
        get => _user;
        set
        {
            _user = value;
            this.OnPropertyChanged(nameof(User));
            if (value != null)
            {
                switch (value.Role.Id)
                {
                    case Role.STUDENT:
                        this.Content = new Page();
                        break;
                    case Role.TEACHER:
                        this.Content = new TeacherPage()
                        {
                            DataContext = new TeacherViewModel() { DataContext = this }
                        };
                        break;
                    case Role.ADMIN:
                        this.Content = new AdministratorPage()
                        {
                            DataContext = new AdministratorViewModel() { DataContext = this }
                        };
                        break;
                }
            }
            else
            {
                this.Content = new AuthorizationPage()
                {
                    DataContext = new AuthorizationViewModel() { DataContext = this }
                };
            }
        }
    }

    private Page _content;

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

    public ICommand ClickCommand { get; set; }

    public ICommand ExitCommand { get; set; }

    public MainViewModel()
    {
        Roles = new ObservableCollection<Role>(Role.getAll());
        Groups = new ObservableCollection<Group>(Group.getAll());
        ClickCommand = new ChangePageCommand(this);
        User = null;
        ExitCommand = new RelayCommand(obj =>
        {
            Content = new Page();
            User = null;
        });
    }
}