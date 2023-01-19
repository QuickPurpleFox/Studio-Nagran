using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Windows.Input;
using System.Reactive;

namespace Szprotify.ViewModels;

public class RegisterWindowViewModel : ViewModelBase
{
    public RegisterWindowViewModel(ConnectDB connect, StyleManager styles)
    {
        RegisterCommandView = ReactiveCommand.Create(() =>
        {
            Console.Write("Register [Username: " + EntryUsername + " ");
            Console.WriteLine("Password: " + EntryPassword + "]");
            connect.Register(EntryUsername, EntryPassword);
        });
        ChangeTheme = ReactiveCommand.Create(() => styles.UseTheme(styles.CurrentTheme switch
        {
            //StyleManager.Theme.Citrus => StyleManager.Theme.Sea,
            //StyleManager.Theme.Sea => StyleManager.Theme.Rust,
            //StyleManager.Theme.Rust => StyleManager.Theme.Candy,
            //StyleManager.Theme.Candy => StyleManager.Theme.Magma,
            //StyleManager.Theme.Magma => StyleManager.Theme.Citrus,
            StyleManager.Theme.Magma => StyleManager.Theme.Rust,
            StyleManager.Theme.Rust => StyleManager.Theme.Magma,
            _ => throw new ArgumentOutOfRangeException(nameof(styles.CurrentTheme))
        }));
    }
    // binding register button
    public ICommand RegisterCommandView {get; } 
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