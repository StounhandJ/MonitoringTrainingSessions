using System;
using System.Windows.Controls;
using System.Windows.Input;
using MonitoringTrainingSessions.Models;
using MonitoringTrainingSessions.ViewModels;

namespace MonitoringTrainingSessions.Commands;

public class AuthorizationCommand : ICommand
{
    private readonly AuthorizationViewModel m_viewModel;

    public AuthorizationCommand(AuthorizationViewModel vm)
    {
        m_viewModel = vm;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        if (string.IsNullOrWhiteSpace(m_viewModel.Login) || m_viewModel.Login.Length > 20)
        {
            m_viewModel.ErrorText = "Неверный формат логина";
            return;
        }

        if (string.IsNullOrWhiteSpace(((PasswordBox)parameter).Password) ||
            ((PasswordBox)parameter).Password.Length > 20)
        {
            m_viewModel.ErrorText = "Неверный формат пароля";
            return;
        }

        User user = User.getByLoginPassword(m_viewModel.Login, ((PasswordBox)parameter).Password);
        if (user.exist())
        {
            m_viewModel.DataContext.User = user;
            return;
        }
        m_viewModel.ErrorText = "Неверный логин или пароль";
    }

    public event EventHandler CanExecuteChanged;
}