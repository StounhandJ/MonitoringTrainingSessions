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
            case "SchedulePage":
                m_viewModel.Content = new SchedulePage()
                {
                    DataContext = new ScheduleViewModel() { DataContext = m_viewModel }
                };
                break;
            case "AdminPage":
                m_viewModel.Content = new AdministratorPage()
                {
                    DataContext = new AdministratorViewModel() { DataContext = m_viewModel }
                };
                break;
            case "TeacherPage":
                m_viewModel.Content = new TeacherPage()
                {
                    DataContext = new TeacherViewModel() { DataContext = m_viewModel }
                };
                break;
            case "StudentPage":
                m_viewModel.Content = new StudentPage()
                {
                    DataContext = new StudentViewModel() { DataContext = m_viewModel }
                };
                break;
            case "LessonTeacherPage":
                m_viewModel.Content = new LessonTeacherPage()
                {
                    DataContext = new LessonTeacherViewModel() { DataContext = m_viewModel }
                };
                break;
            case "LessonStudentPage":
                m_viewModel.Content = new LessonStudentPage()
                {
                    DataContext = new LessonStudentViewModel() { DataContext = m_viewModel }
                };
                break;
            case "TimeSchedulePage":
                m_viewModel.Content = new TimeSchedulePage()
                {
                    DataContext = new TimeScheduleViewModel() { DataContext = m_viewModel }
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