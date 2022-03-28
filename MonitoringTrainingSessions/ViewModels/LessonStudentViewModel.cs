using System;
using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class LessonStudentViewModel : BaseViewModel
{
    public LessonStudentViewModel()
    {
    }

    private MainViewModel _dataContext;

    public MainViewModel DataContext
    {
        get => _dataContext;
        set
        {
            _dataContext = value;
            
            Action act = () => { Lesson = Lesson.getOrCreate(value.User!.CurrentSchedule, DateTime.Now); };
            act.Invoke();
            
            Timer.start(act);
            this.OnPropertyChanged(nameof(DataContext));
        }
    }

    private Lesson _lesson;

    public Lesson Lesson
    {
        get => _lesson;
        set
        {
            _lesson = value;
            this.OnPropertyChanged(nameof(Lesson));
        }
    }
}