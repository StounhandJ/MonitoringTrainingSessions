﻿using System.Windows.Input;
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
    
    private Group _group;

    public Group Group
    {
        get => _group;
        set
        {
            _group = value;
            this.OnPropertyChanged(nameof(Group));
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