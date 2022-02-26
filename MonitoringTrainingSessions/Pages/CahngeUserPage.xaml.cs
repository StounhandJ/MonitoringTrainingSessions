using System.Windows.Controls;
using MonitoringTrainingSessions.Models;
using MonitoringTrainingSessions.ViewModels;

namespace MonitoringTrainingSessions.Pages;

public partial class CahngeUserPage : Page
{
    public CahngeUserPage(User user, Page page, MainViewModel dataContext)
    {
        InitializeComponent();
        DataContext = new CahngeUserViewModel(user, page, dataContext);
    }

    public CahngeUserPage()
    {
    }
}