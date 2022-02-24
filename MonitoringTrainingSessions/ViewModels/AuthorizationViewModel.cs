using System.Windows.Input;
using MonitoringTrainingSessions.Commands;

namespace MonitoringTrainingSessions.ViewModels;

public class AuthorizationViewModel : BaseViewModel
{
    public AuthorizationViewModel()
    {
        _authorizationCommand = new AuthorizationCommand(this);
    }

    private ICommand _authorizationCommand;

    public ICommand AuthorizationCommand
    {
        get { return _authorizationCommand; }
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