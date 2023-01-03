﻿using ReactiveUI;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive;
using System.Windows.Input;

namespace Szprotify.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        ConnectDB connect = new ConnectDB();
        NameOfVariableAndAction = ReactiveCommand.Create(MethodToOpenView);
        //Button LogIn on click
        LogInCommand = ReactiveCommand.Create(() =>
        {
            Console.Write("Logging [Username: " + EntryUsername + " ");
            Console.WriteLine("Password: " + EntryPassword + "]");
            if(connect.Login(EntryUsername, EntryPassword))
            {
                
            }
            else
            {
                
            }
        });

        //Button Register
        RegisterCommand = ReactiveCommand.Create(() =>
        {
            //https://www.reddit.com/r/AvaloniaUI/comments/101h4w0/comment/j2nfqfu/?context=3
            MethodToOpenView();
        });
    }
    
    public ReactiveCommand<Unit, Unit> NameOfVariableAndAction {get; }

    private void MethodToOpenView()
    {
        var window = new Views.RegisterWindow();
        window.Show();
    }

    //Binding from Views to receive input data
    //https://www.reddit.com/r/AvaloniaUI/comments/zxeua8/cant_get_string_from_textbox_binding/
    public ICommand LogInCommand {get; }
    public ICommand RegisterCommand {get; }
    public string EntryUsername {get; set; } = default!;
    public string EntryPassword {get; set; } = default!;

    
}

