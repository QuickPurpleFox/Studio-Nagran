using System;
using System.Reactive;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace Szprotify.ViewModels;

public class ArtistViewModel : ViewModelBase
{
    public ArtistViewModel(String Name)
    {
        DataName = Name;

    }
    // binding button
    private String dataname = String.Empty;
    public String DataName
    {
        get => " "+dataname;
        set => this.RaiseAndSetIfChanged(ref dataname, value);
    }
}