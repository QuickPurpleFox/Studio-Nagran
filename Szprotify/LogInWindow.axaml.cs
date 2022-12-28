using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Szprotify;

public partial class LogInWindow : Window
{
    public LogInWindow()
    {
        // Generated with Avalonia.NameGenerator
        InitializeComponent();
    }
    
    //backing field for Username
    //This is a nullable type.
    //Nullable types allow value types to contain null
    //https://stackoverflow.com/questions/4262240/what-does-one-question-mark-following-a-variable-declaration-mean
    private string? _Username;
    public string? Username
    {
        get
        {
            return _Username;
        }
        set
        {
            //update if username is changed
            if(_Username != value)
            {
                _Username = value;
            }
        }
    }

    public void logInButton_Click(object sender, RoutedEventArgs e)
    {
        var logInButton_Click = (Button)sender;
        logInButton_Click.Content = $"{Username}";
    }
    
}
