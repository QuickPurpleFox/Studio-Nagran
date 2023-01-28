using System;
using System.Reactive;
using System.Collections.ObjectModel;
using ReactiveUI;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
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
    public ShopViewModel(ConnectDB connect, string username, StyleManager.Theme style, Window view)
    { 
        var styleinshop = new StyleManager(view);
        styleinshop.UseTheme(style);

        this.connect = connect;
        connect.getAlbumID(connect.getId(username),ref Albums);
        foreach (int Album_id in Albums)
        {
            SearchResults.Add(new AlbumViewModel(connect.getAlbumName(Album_id), connect.getAlbumArtist(Album_id), "3:27", connect.getAlbumCover(Album_id)));
        }
        Test = "Add album";
    }
    // binding button
    private String test = "Add Album";
    public String Test
    {
        get => test;
        set => this.RaiseAndSetIfChanged(ref test, value);
    }
}