using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands;
using MonitoringTrainingSessions.Lib;
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
            if (value != null && value.exist())
            {
                switch (value.Role.Id)
                {
                    case Role.STUDENT:
                        ClickCommand.Execute("StudentPage");
                        break;
                    case Role.TEACHER:
                        ClickCommand.Execute("TeacherPage");
                        break;
                    case Role.ADMIN:
                        ClickCommand.Execute("AdminPage");
                        break;
                }

                ManagmentSave.saveId(value.Id).Wait();
            }
            else
            {
                this.Content = new AuthorizationPage()
                {
                    DataContext = new AuthorizationViewModel() { DataContext = this }
                };
                ManagmentSave.saveId(-1).Wait();
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
    
    private ObservableCollection<Session> _sessions;

    public ObservableCollection<Session> Sessions
    {
        get => _sessions;
        set
        {
            _sessions = value;
            this.OnPropertyChanged(nameof(Groups));
        }
    }

    public ICommand ClickCommand { get; set; }

    public ICommand ExitCommand { get; set; }

    public MainViewModel()
    {
        ExitCommand = new RelayCommand(obj =>
        {
            Content = new Page();
            User = null;
        });
        Roles = new ObservableCollection<Role>(Role.selectAll());
        Groups = new ObservableCollection<Group>(Group.selectAll());
        Sessions = new ObservableCollection<Session>(Session.selectAll());
        ClickCommand = new ChangePageCommand(this);
        User = User.getById(ManagmentSave.loadId());
        App.DiscordClient.OnActivityJoin += lobby =>
        {
            if (User==null)
                return;

            switch (User.Role.Id)
            {
                case Role.STUDENT:
                    ClickCommand.Execute("LessonStudentPage");
                    break;
                case Role.TEACHER:
                    ClickCommand.Execute("LessonTeacherPage");
                    break;
            }
        };
    }
}