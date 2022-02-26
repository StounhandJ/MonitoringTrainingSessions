using System.Windows.Input;

namespace MonitoringTrainingSessions.Commands.DelegateCommand;

public interface IDelegateCommand : ICommand
{
    void RaiseCanExecuteChanged();
}