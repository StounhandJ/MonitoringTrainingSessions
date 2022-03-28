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

            update();

            Timer.start(update);
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

    private Mark _mark;

    public Mark Mark
    {
        get => _mark;
        set
        {
            _mark = value;
            this.OnPropertyChanged(nameof(Mark));
        }
    }

    private void update()
    {
        Lesson = Lesson.getOrCreate(DataContext.User!.CurrentSchedule, DateTime.Now);
        Mark = Mark.getBySessionUserDate(Lesson.Schedule.Session, DataContext.User!, DateTime.Now);
    }
}