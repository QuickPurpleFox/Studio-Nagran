using System;
using System.Reactive;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;
using System.Collections.Generic;
using Avalonia.Collections;
using Avalonia.Controls;

namespace Szprotify.ViewModels;

public class ShopViewModel : ViewModelBase
{
    public AvaloniaList<AlbumViewModel> SearchResults { get; } = new();
    public List<int> Albums = new List<int>();
    public ConnectDB connect = default!;
    public StyleManager styles = default!;
    public ShopViewModel(ConnectDB connect, string username, StyleManager.Theme style, Window view, ApplicationWindowViewModel app)
    { 
        var styleinshop = new StyleManager(view);
        styleinshop.UseTheme(style);

        this.connect = connect;
        connect.getAllAlbumsID(ref Albums);
        foreach (int Album_id in Albums)
        {
            SearchResults.Add(new AlbumViewModel(connect.getAlbumName(Album_id), connect.getAlbumArtist(Album_id), connect.getSongCount(Album_id), connect.getAlbumCover(Album_id)));
        }
        Test = "Add album";

        BuyAlbum = ReactiveCommand.Create(() =>
        {
            connect.assignAlbum(Albums[SelectedAlbum], connect.getId(username));
            app.populateAlbums(connect);
        });
    }
    // binding button
    private String test = "Add Album";
    public String Test
    {
        get => test;
        set => this.RaiseAndSetIfChanged(ref test, value);
    }
    int selectedalbum = default!;
    public int SelectedAlbum
    {
        get => selectedalbum;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedalbum, value);
        }
    }
    public ICommand BuyAlbum {get; } = default!;
}