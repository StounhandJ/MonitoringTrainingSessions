using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.Pages;

public partial class TeacherPage : Page
{
    public TeacherPage(User user)
    {
        InitializeComponent();
        List<Session> sessions = new List<Session>() { new Session() };
        sessions.AddRange(Session.getAll());
        SessionComboBox.ItemsSource = sessions;
        GroupComboBox.ItemsSource = user.Groups;
        DayComboBox.ItemsSource = new List<int>() { 1, 2, 3, 4, 5 };

        GroupMarksComboBox.ItemsSource = user.Groups;
        SessionMarkComboBox.ItemsSource = sessions;
        MarksComboBox.ItemsSource = new List<int>() { 2, 3, 4, 5 };
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
        }
    }


    private Mark? _mark;

    private void SaveMarkButton_OnClick(object sender, RoutedEventArgs e)
    {
        // if (_isMark)
        // {
        //     _mark.save();
        // }
        // else
        // {
        //     _mark.delete();
        // }
    }

    private void updateMark()
    {
        _mark = Mark.getBySessionUserDate((Session)SessionComboBox.SelectedItem, (User)StudentComboBox.SelectedItem, DatePicker.DisplayDate);
        if (!_mark.exist())
        {
            _mark.date = DatePicker.DisplayDate;
            _mark.Session = (Session)SessionComboBox.SelectedItem;
            _mark.whoWasPutUser = (User)StudentComboBox.SelectedItem;
        }
        else
        {
            MarksComboBox.SelectedItem = _mark.mark;
        }
    }
}