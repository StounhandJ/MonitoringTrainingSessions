using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands;
using MonitoringTrainingSessions.Commands.DelegateCommand;
using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class LessonTeacherViewModel : BaseViewModel
{
    public ICommand SaveCommand { get; set; }
    private static DiscordClient DiscordClient = new DiscordClient(App.DiscordAppId);

    public LessonTeacherViewModel()
    {
        SaveCommand = new DelegateCommand((o) =>
        {
            Lesson.save();
            (new SaveMarksCommand()).Execute(Marks);
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
            if (Lesson.Schedule.exist())
            {
                Lesson.save();
            }
            updateMarks();
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
            if (value.Schedule.exist())
            {
                Visibility = Visibility.Visible;
                DiscordClient.ConnectOrCreateLobbyDiscord(value.discord_id, 50);
                
                if (DiscordClient.IsMute())
                {
                    DiscordClient.Mute();
                }
            }
            else
            {
                Visibility = Visibility.Hidden;
                DiscordClient.LobbySmartDisconnect();
            }

            this.OnPropertyChanged(nameof(Lesson));
        }
    }

    private ObservableCollection<Mark> _marks = new ObservableCollection<Mark>();

    public ObservableCollection<Mark> Marks
    {
        get => _marks;
        set
        {
            _marks = value;
            this.OnPropertyChanged(nameof(Marks));
        }
    }

    private Visibility _visibility = Visibility.Visible;

    public Visibility Visibility
    {
        get => _visibility;
        set
        {
            _visibility = value;
            this.OnPropertyChanged(nameof(Visibility));
        }
    }

    private void updateMarks()
    {
        Marks.Clear();
        foreach (var mark in Mark.getByGroupSessionDate(Lesson.Schedule.Group, Lesson.Schedule.Session,
                     DateTime.Now.Date))
        {
            Marks.Add(mark);
        }

        List<Mark> marks = new List<Mark>(Marks);
        foreach (var user in Lesson.Schedule.Group.Users)
        {
            if (user.Role.Id == Role.STUDENT && !marks.Exists(m => m.whoWasPutUser.Id == user.Id))
            {
                Marks.Add(new Mark(Lesson.Schedule.Session, DataContext.User!, user, null, DateTime.Now.Date));
            }
        }

        foreach (var mark in Marks)
        {
            mark.marks.Insert(0, null);
        }
    }
}