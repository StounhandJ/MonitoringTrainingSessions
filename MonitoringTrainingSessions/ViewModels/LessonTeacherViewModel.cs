using System;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands.DelegateCommand;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class LessonTeacherViewModel : BaseViewModel
{
    public ICommand SaveCommand { get; set; }

    public LessonTeacherViewModel()
    {
        SaveCommand = new DelegateCommand((o)=>
        {
            Lesson.save();
        });
    }

    private MainViewModel _dataContext;

    public MainViewModel DataContext
    {
        get => _dataContext;
        set
        {
            _dataContext = value;
            Lesson = Lesson.getOrCreate(value.User!.CurrentSchedule, DateTime.Now);
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