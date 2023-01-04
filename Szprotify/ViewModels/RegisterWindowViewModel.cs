using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Windows.Input;

namespace Szprotify.ViewModels;

public class RegisterWindowViewModel : ViewModelBase
{
    public RegisterWindowViewModel(ConnectDB connect)
    {
        RegisterCommandView = ReactiveCommand.Create(() =>
        {
            Console.Write("Register [Username: " + EntryUsername + " ");
            Console.WriteLine("Password: " + EntryPassword + "]");
            connect.Register(EntryUsername, EntryPassword);
        });
    }
    // binding register button
    public ICommand RegisterCommandView {get; } 
    public string EntryUsername {get; set; } = default!;
    public string EntryPassword {get; set; } = default!;
}