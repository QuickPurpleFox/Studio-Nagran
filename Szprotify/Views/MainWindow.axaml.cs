using Avalonia.Controls;

namespace Szprotify.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void MethodToOpenView()
    {
        var window = new RegisterWindow();
        window.Show();
    }
}