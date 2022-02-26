using System;
using System.Windows.Input;
using MonitoringTrainingSessions.Pages;
using MonitoringTrainingSessions.ViewModels;

namespace MonitoringTrainingSessions.Commands;

public class ChangePageCommand : ICommand
{
    private readonly MainViewModel m_viewModel;

    public ChangePageCommand(MainViewModel vm)
    {
        m_viewModel = vm;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        switch (parameter)
        {
            case "RegisterPage":
                m_viewModel.Content = new RegisterPage()
                {
                    DataContext = new RegisterViewModel() { DataContext = m_viewModel }
                };


                break;
            case "AuthorizationPage":
                m_viewModel.Content = new AuthorizationPage()
                {
                    DataContext = new AuthorizationViewModel() { DataContext = m_viewModel }
                };

                break;
        }
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}