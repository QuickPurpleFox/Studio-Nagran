using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Szprotify.ViewModels;
using Szprotify.Views;

namespace Szprotify;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    public MainWindow window = default!;
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            //https://www.reddit.com/r/AvaloniaUI/comments/102yss3/button_is_not_working_in_new_opened_window/
            window = new MainWindow();
            window.DataContext = new MainWindowViewModel(new StyleManager(window), this.window);
            desktop.MainWindow = window;
        }

        base.OnFrameworkInitializationCompleted();
    }
}