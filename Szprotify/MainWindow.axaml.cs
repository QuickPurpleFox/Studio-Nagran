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
    //Button to LogOut (close windows)
    public void LogOut_Click(object sender, RoutedEventArgs e)
    {
        Close("Loggin out...");
    }

}