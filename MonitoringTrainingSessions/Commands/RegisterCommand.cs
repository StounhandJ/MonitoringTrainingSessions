using System;
using System.Windows.Controls;
using System.Windows.Input;
using MonitoringTrainingSessions.Models;
using MonitoringTrainingSessions.ViewModels;

namespace MonitoringTrainingSessions.Commands;

public class RegisterCommand : ICommand
{
    private readonly RegisterViewModel m_viewModel;

    public RegisterCommand(RegisterViewModel vm)
    {
        m_viewModel = vm;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        string password = ((PasswordBox)parameter).Password;
        
        if (string.IsNullOrWhiteSpace(m_viewModel.Login) || m_viewModel.Login.Length > 30)
        {
            m_viewModel.ErrorText = "Неверный формат логин";
            return;
        }

        if (string.IsNullOrWhiteSpace(m_viewModel.FIO) || m_viewModel.FIO.Split(' ').Length < 2)
        {
            m_viewModel.ErrorText = "Неверный формат ФИО. Пример: \"Иванов Иван Иванович\" (Отчество не обязательно)";
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            m_viewModel.ErrorText = "Неверный формат паролья";
            return;
        }

        User user = new User(m_viewModel.Login, "", m_viewModel.FIO, m_viewModel.Role);
        user.password = password;
        user.save();
        m_viewModel.DataContext.User = user;
    }

    public event EventHandler CanExecuteChanged;
}