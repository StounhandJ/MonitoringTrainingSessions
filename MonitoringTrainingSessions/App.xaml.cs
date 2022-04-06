using System.Windows;
using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Lib.DB;

namespace MonitoringTrainingSessions
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        public static LogViewer LogViewer = new LogViewer();
        static readonly public long DiscordAppId = 841535948797509642;
        public static DiscordClient DiscordClient = new DiscordClient(App.DiscordAppId);
        protected override void OnStartup(StartupEventArgs e)
        {
            DBConnector.connect("194.169.163.29", "training_sessions", "train_sess", "train_sess228");
            base.OnStartup(e);
            
            // var users = User.selectAll();
            // users[0].save();
            // group.name = "double";
            // group.save();
        }
    }
}