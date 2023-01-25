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
        SongsCount = Time;
    }
    // binding button
    private String datatitle = String.Empty;
    public String DataTitle
    {
        get => " "+datatitle;
        set => this.RaiseAndSetIfChanged(ref datatitle, value);
    }
    private String dataalbum = String.Empty;
    public String DataAlbum
    {
        get => dataalbum;
        set => this.RaiseAndSetIfChanged(ref dataalbum, value);
    }
    private String songscount = String.Empty;
    public String SongsCount
    {
        get => songscount;
        set => this.RaiseAndSetIfChanged(ref songscount, value);
    }
}