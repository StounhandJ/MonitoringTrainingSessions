using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MonitoringTrainingSessions.Models;
using MonitoringTrainingSessions.ViewModels;

namespace MonitoringTrainingSessions.Pages;

public partial class TeacherPage : Page
{
    private MainViewModel dataContext;
    private User _currentUser;

    public TeacherPage()
    {
        InitializeComponent();
    }

    public TeacherPage(User user, MainViewModel dataContext)
    {
        InitializeComponent();
        this.dataContext = dataContext;
        this._currentUser = user;

        List<Session> sessions = new List<Session>() { new Session() };
        sessions.AddRange(Session.getAll());
        SessionComboBox.ItemsSource = sessions;
        GroupComboBox.ItemsSource = user.Groups;
        DayComboBox.ItemsSource = new List<int>() { 1, 2, 3, 4, 5 };

        GroupMarksComboBox.ItemsSource = user.Groups;
        SessionMarkComboBox.ItemsSource = Session.getAll();
        MarksComboBox.ItemsSource = new List<int>() { -1, 2, 3, 4, 5 };
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_isSession)
        {
            _schedule.save();
        }
        else
        {
            _schedule.delete();
        }
    }

    private Group _group;

    private void GroupComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Group? group = (Group?)((ComboBox)sender).SelectedItem;
        if (group != null)
        {
            _group = group;
            updateSchedule();
        }
    }

    private int _day;

    private void DayComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int day = (int)((ComboBox)sender).SelectedItem;
        _day = day;

        updateSchedule();
    }

    private Schedule? _schedule;
    private bool _isSession;

    private void SessionComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Session? session = (Session?)((ComboBox)sender).SelectedItem;
        if (session != null && _schedule != null)
        {
            _schedule.Session = session;
            _isSession = session.exist();
        }
    }

    private void updateSchedule()
    {
        _schedule = Schedule.getByGroupDay(_group, _day);
        if (!_schedule.exist())
        {
            _schedule.Group = _group;
            _schedule.number_day_week = _day;
            SessionComboBox.SelectedItem = null;
        }
        else
        {
            SessionComboBox.SelectedItem = _schedule.Session;
        }
    }


    private void GroupMarksComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Group? group = (Group?)((ComboBox)sender).SelectedItem;
        if (group != null)
        {
            StudentComboBox.ItemsSource = group.Users.FindAll(u => u.Role.Id == Role.STUDENT);
            updateMark();
        }
    }

    private bool _isMark;
    private Mark? _mark;

    private void SaveMarkButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (StudentComboBox.SelectedItem == null || SessionMarkComboBox.SelectedItem == null || DatePicker.SelectedDate==null)
            return;
        if (_isMark)
        {
            _mark.date = DatePicker.SelectedDate ?? new DateTime();
            _mark.Session = (Session)SessionComboBox.SelectedItem;
            _mark.whoWasPutUser = (User)StudentComboBox.SelectedItem;
            _mark.whoPutUser = _currentUser;
            _mark.mark = (int)MarksComboBox.SelectedItem;

            _mark.save();
        }
        else
        {
            _mark.delete();
        }
    }

    private void updateMark()
    {
        if (StudentComboBox.SelectedItem == null || SessionMarkComboBox.SelectedItem == null || DatePicker.SelectedDate==null)
            return;
        _mark = Mark.getBySessionUserDate((Session)SessionMarkComboBox.SelectedItem, (User)StudentComboBox.SelectedItem,
            DatePicker.SelectedDate ?? new DateTime());
        if (_mark.exist())
        {
            MarksComboBox.SelectedItem = _mark.mark;
        }
        else
        {
            MarksComboBox.SelectedItem = -1;
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        this.dataContext.User = null;
    }

    private void DatePicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
    {
        updateMark();
    }

    private void SessionMarkComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        updateMark();
    }

    private void StudentComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        updateMark();
    }

    private void MarksComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _isMark = (int)MarksComboBox.SelectedItem != -1;
    }
}