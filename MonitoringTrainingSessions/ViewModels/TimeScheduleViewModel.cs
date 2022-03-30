using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands.DelegateCommand;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class TimeScheduleViewModel : BaseViewModel
{
    public TimeScheduleViewModel()
    {
        SaveCommand = new DelegateCommand((o) =>
        {
            if (!IsThereError(SelectedDay, StartTime.TimeOfDay, EndTime.TimeOfDay))
            {
                SelectedDay.start_time = StartTime.TimeOfDay;
                SelectedDay.end_time = EndTime.TimeOfDay;
                SelectedDay.save();
            }
        });
        TimeSchedules = new ObservableCollection<TimeSchedule>(TimeSchedule.selectAll());
    }

    public ICommand SaveCommand { get; set; }

    private MainViewModel _dataContext;

    public MainViewModel DataContext
    {
        get => _dataContext;
        set
        {
            _dataContext = value;
            this.OnPropertyChanged(nameof(DataContext));
        }
    }

    private ObservableCollection<TimeSchedule> _timeSchedules;

    public ObservableCollection<TimeSchedule> TimeSchedules
    {
        get => _timeSchedules;
        set
        {
            _timeSchedules = value;
            SelectedDay = value.First();
            this.OnPropertyChanged(nameof(TimeSchedules));
        }
    }

    private TimeSchedule _selectedDay;

    public TimeSchedule SelectedDay
    {
        get => _selectedDay;
        set
        {
            _selectedDay = value;
            StartTime = DateTime.Now.Date + value.start_time;
            EndTime = DateTime.Now.Date + value.end_time;
            this.OnPropertyChanged(nameof(SelectedDay));
        }
    }

    private DateTime _startTime;

    public DateTime StartTime
    {
        get => _startTime;
        set
        {
            _startTime = value;
            this.OnPropertyChanged(nameof(StartTime));

             IsThereError(SelectedDay, StartTime.TimeOfDay, EndTime.TimeOfDay);
        }
    }

    private DateTime _endTime;

    public DateTime EndTime
    {
        get => _endTime;
        set
        {
            _endTime = value;
            this.OnPropertyChanged(nameof(EndTime));

            IsThereError(SelectedDay, StartTime.TimeOfDay, EndTime.TimeOfDay);
        }
    }

    private string _errorText;

    public string ErrorText
    {
        get => _errorText;
        set
        {
            _errorText = value;
            this.OnPropertyChanged(nameof(ErrorText));
        }
    }

    private bool IsThereError(TimeSchedule timeSchedule, TimeSpan new_start_time, TimeSpan new_end_time)
    {
        ErrorText = "";
        if (IsInsideInterval(timeSchedule, new_start_time, new_end_time))
        {
            ErrorText = "Время занятие заходит на другое занятие";
            return true;
        }

        if (IsBetweenInterval(timeSchedule, new_start_time, new_end_time))
        {
            ErrorText = "Время занятие заходит на другое занятие";
            return true;
        }

        if (timeSchedule.start_time > timeSchedule.end_time)
        {
            ErrorText = "Время начала больше конца";
            return true;
        }

        return false;
    }

    private bool IsInsideInterval(TimeSchedule timeSchedule, TimeSpan new_start_time, TimeSpan new_end_time)
    {
        foreach (var vari in TimeSchedules)
        {
            if (vari.Id == timeSchedule.Id)
                continue;

            if (new_start_time > vari.start_time && new_start_time < vari.end_time)
            {
                return true;
            }

            if (new_end_time > vari.start_time && new_end_time < vari.end_time)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsBetweenInterval(TimeSchedule timeSchedule, TimeSpan new_start_time, TimeSpan new_end_time)
    {
        TimeSchedule? lastTimeSchedule = null;
        foreach (var vari in TimeSchedules)
        {
            if (vari.Id == timeSchedule.Id)
            {
                lastTimeSchedule = null;
                continue;
            }


            if (lastTimeSchedule == null)
            {
                lastTimeSchedule = vari;
                continue;
            }

            if (new_start_time >= lastTimeSchedule.end_time && new_start_time <= vari.start_time)
            {
                return true;
            }

            if (new_end_time >= lastTimeSchedule.end_time && new_end_time <= vari.start_time)
            {
                return true;
            }

            lastTimeSchedule = vari;
        }

        return false;
    }
}