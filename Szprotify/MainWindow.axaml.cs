using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Szprotify;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    //Button to LogOut show a dialog "LogInWindow"
    public void LogOut_Click(object sender, RoutedEventArgs e)
    {
        var ownerWindow = this;
        var LogInWindow = new Window();
        LogInWindow.ShowDialog(ownerWindow);
    }

}