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
        if (string.IsNullOrWhiteSpace(m_viewModel.Login) || m_viewModel.Login.Length > 30)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(m_viewModel.FIO) || m_viewModel.FIO.Split(' ').Length < 2)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(m_viewModel.Password))
        {
            return;
        }

        string surname = m_viewModel.FIO.Split(' ')[0];
        string name = m_viewModel.FIO.Split(' ')[1];
        string patronymic = m_viewModel.FIO.Split(' ')[2];

        User user = new User(m_viewModel.Login, m_viewModel.Password, surname, name, patronymic, m_viewModel.Role, m_viewModel.Group);
        user.save();
        m_viewModel.DataContext.Content = new Page();
    }

    public event EventHandler CanExecuteChanged;
}