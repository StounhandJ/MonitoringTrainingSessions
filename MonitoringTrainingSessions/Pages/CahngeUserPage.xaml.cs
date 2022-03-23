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
            return;
        }

        if (string.IsNullOrWhiteSpace(FIOTextBox.Text) || FIOTextBox.Text.Split(' ').Length < 2)
        {
            return;
        }

        this.user.login = LoginTextBox.Text;
        this.user.FIO = FIOTextBox.Text;
        this.user.Role = (Role)RoleComboBox.SelectedItem;
        this.user.Groups = (List<Group>)GroupsCheckComboBox.SelectedItemsOverride;

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
}