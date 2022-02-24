using System.Windows;
using MonitoringTrainingSessions.Lib.DB;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DBConnector.connect("194.169.163.29", "training_sessions", "train_sess", "train_sess228");
            base.OnStartup(e);
            
            var users = User.getAll();
            users[0].save();
            // group.name = "double";
            // group.save();
        }
    }
}