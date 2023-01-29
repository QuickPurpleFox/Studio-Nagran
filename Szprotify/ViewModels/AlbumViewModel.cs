using System;
using ReactiveUI;
using Avalonia.Media.Imaging;
using System.IO;

namespace Szprotify.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    //private readonly Album _album;
    public AlbumViewModel(String Title, String Album, int count, String cover)
    {
        DataTitle = Title;
        DataAlbum = Album;
        SongsCount = count;
        if (File.Exists(Path.Combine(AppContext.BaseDirectory + "/../../../../DataBase/"+cover))) 
        {
            SourceCover = new Bitmap(File.OpenRead(Path.Combine(AppContext.BaseDirectory + "/../../../../DataBase/"+cover)));
        }
        else
        {
            SourceCover =new Bitmap(File.OpenRead(Path.Combine(AppContext.BaseDirectory + "/../../../../Szprotify/Assets/STOCK_EMPTY_COVER.jpg")));
        }
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
    private int songscount = 0;
    public int SongsCount
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