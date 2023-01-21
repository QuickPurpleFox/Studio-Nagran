using System;
using System.Reactive;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace Szprotify.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    //private readonly Album _album;
    public AlbumViewModel(String Title, String Album, String Time)
    {
        DataTitle = Title;
        DataAlbum = Album;
        DataTime = Time;
    }
    // binding button
    private String datatitle = String.Empty;
    public String DataTitle
    {
        get => datatitle;
        set => this.RaiseAndSetIfChanged(ref datatitle, value);
    }
    private String dataalbum = String.Empty;
    public String DataAlbum
    {
        get => dataalbum;
        set => this.RaiseAndSetIfChanged(ref dataalbum, value);
    }
    private String datatime = String.Empty;
    public String DataTime
    {
        get => datatime;
        set => this.RaiseAndSetIfChanged(ref datatime, value);
    }
}