﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MonitoringTrainingSessions.Commands;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class TeacherViewModel : BaseViewModel
{
    public ICommand SaveCommand { get; set; }

    public TeacherViewModel()
    {
        SaveCommand = new SaveGroupsScheduleCommand();
    }

    private ObservableCollection<GroupSchedule> _groupsSchedule = new ObservableCollection<GroupSchedule>();

    public ObservableCollection<GroupSchedule> GroupsSchedule
    {
        get => _groupsSchedule;
        set
        {
            _groupsSchedule = value;
            this.OnPropertyChanged(nameof(GroupsSchedule));
        }
    }

    private List<Group> _groups;

    public List<Group> Groups
    {
        get => _groups;
        set
        {
            _groups = value;
            SelectedDay = 1;
            this.OnPropertyChanged(nameof(Groups));
        }
    }

    private MainViewModel _dataContext;

    public MainViewModel DataContext
    {
        get => _dataContext;
        set
        {
            _dataContext = value;
            Groups = _dataContext.User?.Groups;
            this.OnPropertyChanged(nameof(DataContext));
        }
    }

    private int _selectedDay;

    public int SelectedDay
    {
        get => _selectedDay;
        set
        {
            _selectedDay = value;

            GroupsSchedule.Clear();
            foreach (var group in Groups)
            {
                GroupsSchedule.Add(new GroupSchedule(group, Schedule.getByGroupDay(group, SelectedDay),
                    new List<Session>(DataContext.Sessions), SelectedDay));
            }

            this.OnPropertyChanged(nameof(SelectedDay));
        }
    }
}