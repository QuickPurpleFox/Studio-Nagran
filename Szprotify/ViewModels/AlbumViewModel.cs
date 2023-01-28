using System;
using static System.Uri;
using System.Reactive;
using System.Collections.ObjectModel;
using ReactiveUI;
using Avalonia.Media.Imaging;
using System.IO;

namespace Szprotify.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    //private readonly Album _album;
    public AlbumViewModel(String Title, String Album, String count, String cover)
    {
        DataTitle = Title;
        DataAlbum = Album;
        SongsCount = count;
        SourceCover = new Bitmap(File.OpenRead(@"../DataBase/"+cover));
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
    private Bitmap sourcecover = default!;
    public Bitmap SourceCover
    {
        get => sourcecover;
        set => this.RaiseAndSetIfChanged(ref sourcecover, value);
    }
}