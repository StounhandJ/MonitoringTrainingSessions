using System.Collections.Generic;
using System.Windows;
using MonitoringTrainingSessions.Lib;
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
            DB.connect("194.169.163.29", "training_sessions", "train_sess", "train_sess228");
            base.OnStartup(e);
            var m = new User()
            {
                role_id = 1,
                group_id = 1,

                name = "f",
                surname = "f",
                patronymic = "f",
                login = "f",
                password = "f",
            };
            
        }
    }
}