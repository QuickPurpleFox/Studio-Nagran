using System;
using System.Reactive;
using ReactiveUI;

namespace Szprotify.ViewModels;

public class ApplicationWindowViewModel : ViewModelBase
{
    public UserData user = default!;
    public ApplicationWindowViewModel(ConnectDB connect, StyleManager styles, string username)
    {
        user = new UserData(connect.getRole(username), connect.getId(username), username);        
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
    // binding button
    public ReactiveCommand<Unit, Unit> ChangeTheme { get; } = default!;
    private string printusername = string.Empty;
    public string PrintUsername
    {
        get
        {
            return "Hello: "+user.Username;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref printusername, value);
            PrintUsername = user.Username;
        }
    }
}