using System;
using System.Diagnostics;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Windows.Input;
using ReactiveUI;
using System.Collections.Generic;
using Avalonia.Collections;
using Avalonia.Controls;
using System.IO;

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
            SearchResults.Add(new AlbumViewModel(connect.getAlbumName(Album_id), connect.getAlbumArtist_id(Album_id), connect.getSongCount(Album_id), connect.getAlbumCover(Album_id)));
        }
        Test = "Add album";

        BuyAlbum = ReactiveCommand.Create(() =>
        {
            connect.assignAlbum(Albums[SelectedAlbum], connect.getId(username));
            app.populateAlbums(connect);
            Document.Create(container =>
            {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));
        
                page.Header()
                .Text("Hello PDF!")
                .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);
        
                page.Content()
                .PaddingVertical(1, Unit.Centimetre)
                .Column(x =>
                {
                    x.Spacing(20);
                
                    x.Item().Text(Placeholders.LoremIpsum());
                    x.Item().Image(Placeholders.Image(200, 100));
                });
        
                page.Footer()
                .AlignCenter()
                .Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
            });
        }).GeneratePdf("hello.pdf");
        Process.Start(Path.Combine(AppContext.BaseDirectory + "/../../../../Szprotify/") +"hello.pdf");
        });

        Invoice = ReactiveCommand.Create(()=>
        {
            Process p = new Process();
            p.StartInfo.FileName = @"D:\Studio_Nagran\Studio-Nagran\Szprotify\hello.pdf";
            p.Start();
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
    public ICommand Invoice {get; } = default!;
}