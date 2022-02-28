using System.Windows.Input;
using MonitoringTrainingSessions.Commands;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    public RegisterViewModel()
    {
        _registerCommand = new RegisterCommand(this);
    }

    private ICommand _registerCommand;

    public ICommand RegisterCommand
    {
        get { return _registerCommand; }
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

    private string _fio;

    public string FIO
    {
        get => _fio;
        set
        {
            _fio = value;
            this.OnPropertyChanged(nameof(FIO));
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