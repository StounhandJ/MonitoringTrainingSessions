using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MonitoringTrainingSessions.Models;

namespace MonitoringTrainingSessions.ViewModels;

public class StudentViewModel : BaseViewModel
{
    public StudentViewModel()
    {
        Marks = new ObservableCollection<Mark>();
    }

    private MainViewModel _dataContext;

    public MainViewModel DataContext
    {
        get => _dataContext;
        set
        {
            _dataContext = value;
            SelectedDate = DateTime.Now;
            this.OnPropertyChanged(nameof(DataContext));
        }
    }


    private DateTime? _selectedDate;

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            if (_selectedDate != null)
            {
                Marks.Clear();
                foreach (var mark in Mark.getByUserDate(DataContext.User!, SelectedDate ?? new DateTime()))
                {
                    Marks.Add(mark);
                }

                List<Mark> marks = new List<Mark>(Marks);
                foreach (var session in DataContext.Sessions)
                {
                    if (!marks.Exists(m => m.Session.Id == session.Id))
                    {
                        Marks.Add(new Mark(session, DataContext.User!, DataContext.User!, null,
                            SelectedDate ?? new DateTime()));
                    }
                }
            }

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
}