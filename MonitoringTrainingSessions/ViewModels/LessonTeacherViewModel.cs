﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands;
using MonitoringTrainingSessions.Commands.DelegateCommand;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class LessonTeacherViewModel : BaseViewModel
{
    public ICommand SaveCommand { get; set; }

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