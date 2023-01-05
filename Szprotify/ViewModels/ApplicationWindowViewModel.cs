using System;
using System.Reactive;
using ReactiveUI;

namespace Szprotify.ViewModels;

public class ApplicationWindowViewModel : ViewModelBase
{
    public ApplicationWindowViewModel(ConnectDB connect, StyleManager styles)
    {        

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
}