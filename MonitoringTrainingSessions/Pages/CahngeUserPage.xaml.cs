using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MonitoringTrainingSessions.Models;
using MonitoringTrainingSessions.ViewModels;

namespace MonitoringTrainingSessions.Pages;

public partial class CahngeUserPage : Page
{
    private User user;
    private Page page;
    private MainViewModel dataContext;

    public CahngeUserPage(User user, Page page, MainViewModel dataContext)
    {
        InitializeComponent();
        this.LoginTextBox.Text = user.login;
        this.FIOTextBox.Text = user.FIO;
        this.FIOTextBox.Text = user.FIO;
        this.RoleComboBox.ItemsSource = dataContext.Roles;
        this.RoleComboBox.SelectedItem = user.Role;
        this.GroupsCheckComboBox.ItemsSource = dataContext.Groups;
        this.GroupsCheckComboBox.SelectedItemsOverride = user.Groups;
        this.GroupsCheckComboBox.Visibility = user.Role.Id==Role.TEACHER?Visibility.Visible:Visibility.Hidden;
        this.GroupsCheckComboBox2.ItemsSource = dataContext.Groups;
        this.GroupsCheckComboBox2.SelectedItem = user.Groups;
        this.GroupsCheckComboBox2.Visibility =  user.Role.Id==Role.STUDENT?Visibility.Visible:Visibility.Hidden;;
        this.user = user;
        this.page = page;
        this.dataContext = dataContext;
    }

    public CahngeUserPage()
    {
    }


    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || LoginTextBox.Text.Length > 30)
        {
            ErrorTextBlock.Text = "Неверный логин или пароль";
            return;
        }
        
        if (!User.getByLogin(LoginTextBox.Text).exist())
        {
            ErrorTextBlock.Text = "Данный логин уже занят";
            return;
        }

        if (string.IsNullOrWhiteSpace(FIOTextBox.Text) || FIOTextBox.Text.Split(' ').Length < 2)
        {
            ErrorTextBlock.Text = "Неверный формат ФИО. Пример: \"Иванов Иван Иванович\" (Отчество не обязательно)";
            return;
        }

        this.user.login = LoginTextBox.Text;
        this.user.FIO = FIOTextBox.Text;
        this.user.Role = (Role)RoleComboBox.SelectedItem;
        if (user.Role.Id==Role.TEACHER)
        {
            this.user.Groups = (List<Group>)GroupsCheckComboBox.SelectedItemsOverride;
        }
        else if (user.Role.Id==Role.STUDENT)
        {
            this.user.Groups = new List<Group>(){(Group)GroupsCheckComboBox2.SelectedItem};
        }
        

        if (PasswordTextBox.Text != "")
        {
            this.user.password = PasswordTextBox.Text;
        }

        this.user.save();

        dataContext.Content = page;
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.user.delete();
        dataContext.Content = page;
    }

    private void RoleComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        this.GroupsCheckComboBox.Visibility = ((Role)RoleComboBox.SelectedItem).Id==Role.TEACHER?Visibility.Visible:Visibility.Hidden;
        this.GroupsCheckComboBox2.Visibility =  ((Role)RoleComboBox.SelectedItem).Id==Role.STUDENT?Visibility.Visible:Visibility.Hidden;;
    }
}