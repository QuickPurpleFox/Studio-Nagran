using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Windows.Input;

namespace Szprotify.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        //Button LogIn on click
        LogInCommand = ReactiveCommand.Create(() =>
        {
            Console.Write(EntryUsername + " ");
            Console.WriteLine(EntryPassword);
        });
    }

    //Binding from Views to receive input data
    //https://www.reddit.com/r/AvaloniaUI/comments/zxeua8/cant_get_string_from_textbox_binding/
    public ICommand LogInCommand {get; }
    public string EntryUsername {get; set; } = default!;
    public string EntryPassword {get; set; } = default!;
    
}

