using ReactiveUI;
using Avalonia.Controls;
using Avalonia.Themes.Fluent;
using Avalonia;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;

namespace Szprotify.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ConnectDB connect;
    public MainWindowViewModel(StyleManager styles)
    {
        connect = new ConnectDB();
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
        // Each time a user clicks 'Switch theme', we load next theme. See 'StyleManager.cs'.
        ChangeTheme = ReactiveCommand.Create(() => styles.UseTheme(styles.CurrentTheme switch
        {
            StyleManager.Theme.Citrus => StyleManager.Theme.Sea,
            StyleManager.Theme.Sea => StyleManager.Theme.Rust,
            StyleManager.Theme.Rust => StyleManager.Theme.Candy,
            StyleManager.Theme.Candy => StyleManager.Theme.Magma,
            StyleManager.Theme.Magma => StyleManager.Theme.Citrus,
            _ => throw new ArgumentOutOfRangeException(nameof(styles.CurrentTheme))
        }));
    }
    
    public ReactiveCommand<Unit, Unit> NameOfVariableAndAction {get; }

    private void MethodToOpenView()
    {
        var RegisterWindow = new Views.RegisterWindow();
        RegisterWindow.DataContext = new RegisterWindowViewModel(connect);
        RegisterWindow.Show();
    }

    //Binding from Views to receive input data
    //https://www.reddit.com/r/AvaloniaUI/comments/zxeua8/cant_get_string_from_textbox_binding/
    public ICommand LogInCommand {get; }
    public ICommand RegisterCommand {get; }
    public string EntryUsername {get; set; } = default!;
    public string EntryPassword {get; set; } = default!;
    public ReactiveCommand<Unit, Unit> ChangeTheme { get; } = default!;

    
}

