using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class TeacherViewModel : BaseViewModel
{
    public ICommand SaveCommand { get; set; }

    public TeacherViewModel()
    {
        SaveCommand = new SaveMarksCommand();
        Marks = new ObservableCollection<Mark>();
        SelectedDate = DateTime.Now;
    }

    private MainViewModel _dataContext;

    public MainViewModel DataContext
    {
        get => _dataContext;
        set
        {
            _dataContext = value;
            SelectedGroup = value.Groups.First();
            SelectedSession = value.Sessions.First();
            this.OnPropertyChanged(nameof(DataContext));
        }
    }

    private Group _selectedGroup = new Group();

    public Group SelectedGroup
    {
        get => _selectedGroup;
        set
        {
            _selectedGroup = value;
            DataContext.AdditionalCommandParametr = value;
            update();
            this.OnPropertyChanged(nameof(SelectedGroup));
        }
    }

    private Session _selectedSession = new Session();

    public Session SelectedSession
    {
        get => _selectedSession;
        set
        {
            _selectedSession = value;
            update();
            this.OnPropertyChanged(nameof(SelectedSession));
        }
    }

    private DateTime? _selectedDate;

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value!.Value.Date;
            update();
            this.OnPropertyChanged(nameof(SelectedDate));
        }
    }

    private ObservableCollection<Mark> _marks;

    public ObservableCollection<Mark> Marks
    {
        get => _marks;
        set
        {
            _marks = value;
            this.OnPropertyChanged(nameof(Marks));
        }
    }

    private void update()
    {
        if (SelectedGroup==null || !SelectedGroup.exist() || !SelectedSession.exist() || SelectedDate == null)
            return;

        Marks.Clear();
        foreach (var mark in Mark.getByGroupSessionDate(SelectedGroup, SelectedSession, SelectedDate ?? new DateTime()))
        {
            Marks.Add(mark);
        }

        List<Mark> marks = new List<Mark>(Marks);
        foreach (var user in SelectedGroup.Users)
        {
            if (user.Role.Id == Role.STUDENT && !marks.Exists(m => m.whoWasPutUser.Id == user.Id))
            {
                Marks.Add(new Mark(SelectedSession, DataContext.User!, user, null, SelectedDate ?? new DateTime()));
            }
        }

        foreach (var mark in Marks)
        {
            mark.marks.Insert(0, null);
        }
    }
}