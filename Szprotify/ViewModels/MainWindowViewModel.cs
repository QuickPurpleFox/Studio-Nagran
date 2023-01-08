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
        OpenRegister = ReactiveCommand.Create(MethodToOpenRegister);
        OpenAplication = ReactiveCommand.Create(MethodToOpenApplication);
        //Button LogIn on click
        LogInCommand = ReactiveCommand.Create(() =>
        {
            Console.Write("Logging [Username: " + EntryUsername + " ");
            Console.WriteLine("Password: " + EntryPassword + "]");
            if(connect.Login(EntryUsername, EntryPassword))
            {
                MethodToOpenApplication();
            }
            else
            {
                
            }
        });

        //Button Register
        RegisterCommand = ReactiveCommand.Create(() =>
        {
            //https://www.reddit.com/r/AvaloniaUI/comments/101h4w0/comment/j2nfqfu/?context=3
            MethodToOpenRegister();
        });
        // Each time a user clicks 'Switch theme', we load next theme. See 'StyleManager.cs'.
        ChangeTheme = ReactiveCommand.Create(() => styles.UseTheme(styles.CurrentTheme switch
        {
            //StyleManager.Theme.Citrus => StyleManager.Theme.Sea,
            //StyleManager.Theme.Sea => StyleManager.Theme.Rust,
            //StyleManager.Theme.Rust => StyleManager.Theme.Candy,
            //StyleManager.Theme.Candy => StyleManager.Theme.Magma,
            //StyleManager.Theme.Magma => StyleManager.Theme.Citrus,
            StyleManager.Theme.Magma => StyleManager.Theme.Sea,
            StyleManager.Theme.Sea => StyleManager.Theme.Magma,
            _ => throw new ArgumentOutOfRangeException(nameof(styles.CurrentTheme))
        }));
    }
    
    public ReactiveCommand<Unit, Unit> OpenRegister {get; }
    public ReactiveCommand<Unit, Unit> OpenAplication {get; }
    private void MethodToOpenRegister()
    {
        var RegisterWindow = new Views.RegisterWindow();
        var styles = new StyleManager(RegisterWindow);
        RegisterWindow.DataContext = new RegisterWindowViewModel(connect, styles);
        RegisterWindow.Show();
    }
    private void MethodToOpenApplication()
    {
        var ApplicationWindow = new Views.ApplicationWindow();
        var styles = new StyleManager(ApplicationWindow);
        ApplicationWindow.DataContext = new ApplicationWindowViewModel(connect, styles, EntryUsername);
        ApplicationWindow.Show();
    }

    //Binding from Views to receive input data
    //https://www.reddit.com/r/AvaloniaUI/comments/zxeua8/cant_get_string_from_textbox_binding/
    public ICommand LogInCommand {get; }
    public ICommand RegisterCommand {get; }
    private string entryusername = string.Empty;
    public string EntryUsername
    {
        get
        {
            return entryusername;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref entryusername, value);
            if (EntryUsername.Length >= 5 && EntryPassword.Length >= 5)
            {
                Enable = true;
            }
            else
            {
                Enable = false;
            }
        }
    }
    //public string EntryPassword {get; set; } = default!;
    private string entrypassword = string.Empty;
    public string EntryPassword
    {
        get
        {
            return entrypassword;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref entrypassword, value);
            if (EntryPassword.Length >= 5 && EntryUsername.Length >= 5)
            {
                Enable = true;
            }
            else
            {
                Enable = false;
            }
        }
    }
    public ReactiveCommand<Unit, Unit> ChangeTheme { get; } = default!;
    private bool enable = false;
    public bool Enable
    {
        get => enable;
        set => this.RaiseAndSetIfChanged(ref enable, value);
    }

    
}

