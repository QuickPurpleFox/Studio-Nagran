using System;
using System.Reactive;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;
using System.Collections.Generic;
using Avalonia.Collections;
using Avalonia.Controls;

namespace Szprotify.ViewModels;

public class ManagementModel : ViewModelBase
{
    public AvaloniaList<UserViewModel> SearchResults { get; } = new();
    public List<String> Users = new List<String>();
    public ConnectDB connect = default!;
    public ManagementModel(ConnectDB connect, string username, StyleManager.Theme style, Window view, ApplicationWindowViewModel app)
    { 
        var styleinview = new StyleManager(view);
        styleinview.UseTheme(style);

        this.connect = connect;
        connect.getAllUsersName(ref Users);
        foreach (String User_name in Users)
        {   
            SearchResults.Add(new UserViewModel(User_name, connect.getRole(User_name)));
        }

        ChangeRole = ReactiveCommand.Create(() =>
        {
            errorMessage = Users[SelectedUser]+" Role changed";
            if (Users[SelectedUser] == username)
            {
                errorMessage = "Can't change your own Role";
            }
            else if (connect.getRole(Users[SelectedUser]) == "Admin")
            {
                connect.changeRole(Users[SelectedUser], "User");
            }
            else
            {
                connect.changeRole(Users[SelectedUser], "Admin");
            }
            SearchResults.Clear();
            Users.Clear();
            connect.getAllUsersName(ref Users);
            foreach (String User_name in Users)
            {   
                SearchResults.Add(new UserViewModel(User_name, connect.getRole(User_name)));
            }
        });

        AdminResign = ReactiveCommand.Create(() =>
        {
            if(connect.countAdmins() == 1)
            {
                errorMessage = "Can't change role \n need at least one additinal admin";
            }
            else
            {
                connect.changeRole(username, "User");
                view.Close();
            }
        });
    }
    // binding button
    public ICommand ChangeRole {get; }
    public ICommand AdminResign {get; }
    private int selecteduser = default!;
    public int SelectedUser
    {
        get => selecteduser;
        set
        {
            this.RaiseAndSetIfChanged(ref selecteduser, value);
        }
    }
    private String errormessage = String.Empty;
    public String errorMessage
    {
        get => errormessage;
        set
        {
            this.RaiseAndSetIfChanged(ref errormessage, value);
        }
    }
}