using System.Windows.Controls;
using MonitoringTrainingSessions.Lib.DB;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.Pages;

public partial class AdministratorPage : Page
{
    public AdministratorPage()
    {
        InitializeComponent();
    }

    private void DataGridGroup_OnRowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
    {
        var g = (Group)e.Row.DataContext;
        g.save();
    }

    private void DataGridSession_OnRowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
    {
        var s = (Session)e.Row.DataContext;
        s.save();
    }
}