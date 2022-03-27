using System.Collections.Generic;
using System.Windows.Controls;

namespace MonitoringTrainingSessions.Pages;

public partial class SchedulePage : Page
{
    public SchedulePage()
    {
        InitializeComponent();
        DayComboBox.ItemsSource = new List<int> { 1, 2, 3, 4, 5 };
    }
}