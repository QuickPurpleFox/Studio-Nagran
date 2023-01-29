using System;
using static System.Uri;
using System.Reactive;
using System.Collections.ObjectModel;
using ReactiveUI;
using Avalonia.Media.Imaging;
using System.IO;

namespace Szprotify.ViewModels;

public class UserViewModel : ViewModelBase
{
    //private readonly Album _album;
    public UserViewModel(String Name, String Role)
    {
        DataName = Name;
        DataRole = Role;
    }
    // binding button
    private String dataname = String.Empty;
    public String DataName
    {
        get => " "+dataname;
        set => this.RaiseAndSetIfChanged(ref dataname, value);
    }
    private String datarole = String.Empty;
    public String DataRole
    {
        get => datarole;
        set => this.RaiseAndSetIfChanged(ref datarole, value);
    }
}