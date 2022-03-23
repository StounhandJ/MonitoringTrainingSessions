using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MonitoringTrainingSessions.Converters;

public class GroupToLessonPairConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return "1";
    }
     
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }  
}