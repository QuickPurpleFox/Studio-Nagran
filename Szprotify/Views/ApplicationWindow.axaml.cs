using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Controls;
using ReactiveUI;

namespace Szprotify.Views;

public partial class ApplicationWindow : Window
{
    public ApplicationWindow()
    {
        InitializeComponent();
        AvaloniaXamlLoader.Load(this);
    }
}