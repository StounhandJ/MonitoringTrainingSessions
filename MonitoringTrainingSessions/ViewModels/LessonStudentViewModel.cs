﻿using System;
using System.Windows;
using MonitoringTrainingSessions.Lib;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class LessonStudentViewModel : BaseViewModel
{
    
    private static DiscordClient DiscordClient = new DiscordClient(App.DiscordAppId);
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
            if (value.Schedule.exist())
            {
                Visibility = Visibility.Visible;
                DiscordClient.ConnectOrCreateLobbyDiscord(value.discord_id, 50);
            }
            else
            {
                Visibility = Visibility.Hidden;
                DiscordClient.LobbySmartDisconnect();
            }
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
    
    private Visibility _visibility = Visibility.Visible;

    public Visibility Visibility
    {
        get => _visibility;
        set
        {
            _visibility = value;
            this.OnPropertyChanged(nameof(Visibility));
            this.OnPropertyChanged(nameof(NoVisibility));
        }
    }
    
    public Visibility NoVisibility
    {
        get => Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
    }

    private void update()
    {
        Lesson = Lesson.getOrCreate(DataContext.User!.CurrentSchedule, DateTime.Now);
        Mark = Mark.getBySessionUserDate(Lesson.Schedule.Session, DataContext.User!, DateTime.Now);
    }
}