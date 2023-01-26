using System;
using System.Reactive;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Avalonia.Collections;
using ReactiveUI;
using System.Windows.Input;

namespace Szprotify.ViewModels;

public class ApplicationWindowViewModel : ViewModelBase
{
    public AvaloniaList<AlbumViewModel> SearchResults { get; } = new();
    public AvaloniaList<SongViewModel> SongResults { get; } = new();
    public UserData user = default!;
    public string[] lines = default!;
    public List<int> Albums = new List<int>();
    public List<int> Songs = new List<int>();
    public ApplicationWindowViewModel(ConnectDB connect, StyleManager styles, string username)
    {
        //SearchResults.Add(new AlbumViewModel("Infected", "STARSET", "3:08"));
        //SearchResults.Add(new AlbumViewModel("My Heart I Surrender", "I Prevail", "3:27"));
        
        connect.getAlbumID(connect.getId(username),ref Albums);
        /*
        foreach (int Album_id in Albums)
        {
            connect.getSongID(Album_id,ref Songs);
        }

        foreach (int Album_id in Albums)
        {
            SearchResults.Add(new AlbumViewModel(connect.getAlbumName(Album_id), connect.getAlbumArtist(Album_id), "3:27"));
        }

        foreach (int Song_id in Songs)
        {
            SongResults.Add(new SongViewModel(connect.getSongName(Song_id), connect.getSongAlbum(Song_id), connect.getSongDuration(Song_id)));
        }
        */
        populateSongs(connect);
        //SongResults.Clear();

        user = new UserData(connect.getRole(username), connect.getId(username), username); 
            if (user.Role == "Admin")
            {
                IsVisibleAdmin = true;
            }
            else
            {
                IsVisibleAdmin = false;
            }
        PrintUsername = "User: "+user.Username;     
        ChangeTheme = ReactiveCommand.Create(() => styles.UseTheme(styles.CurrentTheme switch
        {
            StyleManager.Theme.Magma => StyleManager.Theme.Rust,
            StyleManager.Theme.Rust => StyleManager.Theme.Magma,
            _ => throw new ArgumentOutOfRangeException(nameof(styles.CurrentTheme))
        }));
        PolishLaunguage = ReactiveCommand.Create(() =>
        {
            PrintUsername = "Użytkownik: "+user.Username;
            lines = System.IO.File.ReadAllLines(@"Assets/pl_pl.txt");
            //change language to polish
            SwitchThemeButton = lines[0];
            SwitchLanguageButton = lines[1];
            ProfileButtonText = lines[2];
            ShopButtonText = lines[3];
            AddAlbumButtonText = lines[4];
            UserManagementButtonText = lines[5];
            TitleBoxText = lines[6];
            TimeBoxText = lines[7];
            ArtistBoxText = lines[8];
        });
        EnglishLaunguage = ReactiveCommand.Create(() =>
        {
            PrintUsername = "User: "+user.Username;
            lines = System.IO.File.ReadAllLines(@"Assets/en_us.txt");
            //change language to English
            SwitchThemeButton = lines[0];
            SwitchLanguageButton = lines[1];
            ProfileButtonText = lines[2];
            ShopButtonText = lines[3];
            AddAlbumButtonText = lines[4];
            UserManagementButtonText = lines[5];
            TitleBoxText = lines[6];
            TimeBoxText = lines[7];
            ArtistBoxText = lines[8];
        });    
    }
    // binding button
    public ICommand PolishLaunguage {get; } = default!;
    public ICommand EnglishLaunguage {get; } = default!;
    public ReactiveCommand<Unit, Unit> ChangeTheme { get; } = default!;
    private string printusername = string.Empty;
    public string PrintUsername
    {
        get
        {
            return printusername;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref printusername, value);
        }
    }
    private bool isvisibleadmin = false;
    public bool IsVisibleAdmin
    {
        get => isvisibleadmin;
        set => this.RaiseAndSetIfChanged(ref isvisibleadmin, value);
    }

    int selectedalbum = default!;
    public int SelectedAlbum
    {
        get => selectedalbum;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedalbum, value);
            SongResults.Clear(); 
        }
    }

    public void populateSongs(ConnectDB connect)
    {
        connect.getSongID(Albums[SelectedAlbum],ref Songs);

        foreach (int Album_id in Albums)
        {
            SearchResults.Add(new AlbumViewModel(connect.getAlbumName(Album_id), connect.getAlbumArtist(Album_id), "3:27"));
        }

        foreach (int Song_id in Songs)
        {
            SongResults.Add(new SongViewModel(connect.getSongName(Song_id), connect.getSongAlbum(Song_id), connect.getSongDuration(Song_id)));
        }
    }










    //----------------------------------LANGUAGE-BINDING----------------------------------
    private String switchlanguagebutton = "Switch Language";
    public String SwitchLanguageButton
    {
        get => switchlanguagebutton;
        set => this.RaiseAndSetIfChanged(ref switchlanguagebutton, value);
    }
    private String switchthemebutton = "Switch theme";
    public String SwitchThemeButton
    {
        get => switchthemebutton;
        set => this.RaiseAndSetIfChanged(ref switchthemebutton, value);
    }
    private String profilebuttontext = "Profile";
    public String ProfileButtonText
    {
        get => profilebuttontext;
        set => this.RaiseAndSetIfChanged(ref profilebuttontext, value);
    }
    private String shopbuttontext = "Shop";
    public String ShopButtonText
    {
        get => shopbuttontext;
        set => this.RaiseAndSetIfChanged(ref shopbuttontext, value);
    }
    private String addalbumbuttontext = "Add Album";
    public String AddAlbumButtonText
    {
        get => addalbumbuttontext;
        set => this.RaiseAndSetIfChanged(ref addalbumbuttontext, value);
    }
    private String usermanagementbuttontext = "User Management";
    public String UserManagementButtonText
    {
        get => usermanagementbuttontext;
        set => this.RaiseAndSetIfChanged(ref usermanagementbuttontext, value);
    }
    private String titleboxtext = "Title";
    public String TitleBoxText
    {
        get => titleboxtext;
        set => this.RaiseAndSetIfChanged(ref titleboxtext, value);
    }
    private String artistboxtext = "Artist";
    public String ArtistBoxText
    {
        get => artistboxtext;
        set => this.RaiseAndSetIfChanged(ref artistboxtext, value);
    }
    private String timeboxtext = "Time";
    public String TimeBoxText
    {
        get => timeboxtext;
        set => this.RaiseAndSetIfChanged(ref timeboxtext, value);
    }
    //-------------------------------------------------------------------------------------
}