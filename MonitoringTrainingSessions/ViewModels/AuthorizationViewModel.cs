using System.Windows.Input;
using MonitoringTrainingSessions.Commands;

namespace MonitoringTrainingSessions.ViewModels;

public class AuthorizationViewModel : BaseViewModel
{
    public AuthorizationViewModel()
    {
        AuthorizationCommand = new AuthorizationCommand(this);
    }

    public ICommand AuthorizationCommand
    {
        get;
        set;
    }

    private string _login;

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            
            this.OnPropertyChanged(nameof(Login));
        }
    }

    private string _password;

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            this.OnPropertyChanged(nameof(Password));
        }
    }


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