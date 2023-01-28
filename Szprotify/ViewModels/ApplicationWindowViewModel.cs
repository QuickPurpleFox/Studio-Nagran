using System;
using System.IO;
using System.Reactive;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Avalonia.Collections;
using Avalonia.Controls;
using ReactiveUI;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Szprotify.ViewModels;

public class ApplicationWindowViewModel : ViewModelBase
{
    public AvaloniaList<AlbumViewModel> SearchResults { get; } = new();
    public AvaloniaList<SongViewModel> SongResults { get; } = new();
    public UserData user = default!;
    public string[] lines = default!;
    public List<int> Albums = new List<int>();
    public List<int> Songs = new List<int>();
    public ConnectDB connect = default!;
    public string username = string.Empty;
    public Task<string[]?> CoverPathAsync = default!;
    public string CoverPath = default!;
    public List<String> ArtistsName = new List<String>();
    public Interaction<ShopViewModel, AlbumViewModel?> ShowDialog { get; }
    public ApplicationWindowViewModel(ConnectDB connect, StyleManager styles, string username, Views.ApplicationWindow ApplicationWindow)
    {
        this.username = username;
        this.connect = connect;
        //connect.getAlbumID(connect.getId(username),ref Albums);
        connect.getArtistName(ref ArtistsName);
        if(connect.getRole(username) == "Admin")
        {
            connect.getAllAlbumsID(ref Albums);
        }
        else
        {
            connect.getAlbumID(connect.getId(username),ref Albums);
        }
        if(Albums.Count != 0)
        {
            populateAlbums(connect);
            populateSongs(connect);
        }

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
        

        AlbumCoverPath = ReactiveCommand.Create(() =>
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a file";
            openFileDialog.InitialFileName = "C:\\";
            openFileDialog.Filters.Add(new FileDialogFilter { Name = "jpeg files", Extensions = {"jpg"}});
            CoverPathAsync = openFileDialog.ShowAsync(ApplicationWindow);
            var UwU = CoverPathAsync.Result;
            CoverPath = Path.GetFileName(UwU[0]);
            var destinationPath = Path.Combine(@"../DataBase", Path.GetFileName(UwU[0]));
            File.Copy(UwU[0],destinationPath);
        });  
        Artists = ArtistsName; 

        AddAlbum = ReactiveCommand.Create(() =>
        {
            if (CoverPath == default!)
            {
                CoverPath = "STOCK_EMPTY_COVER.jpg";
            }
            connect.addAlbum(CoverPath, EntryAlbumName, SelectedArtist);
            //connect.assignAlbum(EntryAlbumName, connect.getId(username));
            connect.assignAlbum(connect.getAlbumIDfromName(EntryAlbumName), connect.getId(username));
            SearchResults.Clear();
            populateAlbums(connect);
        });

        DeleteAlbum = ReactiveCommand.Create(() =>
        {
            connect.deleteAlbum(Albums[SelectedAlbum]);
            SearchResults.Clear();
            populateAlbums(connect);
        });



        AddSong = ReactiveCommand.Create(() =>
        {
            connect.addSong(Albums[SelectedAlbum], EntrySongName, EntrySongDuration);
            SongResults.Clear();
            populateSongs(connect);
        });

        DeleteSong = ReactiveCommand.Create(() =>
        {
            connect.deleteSong(Songs[SelectedSong]);
            SearchResults.Clear();
            populateAlbums(connect);
        });


        ShowDialog = new Interaction<ShopViewModel, AlbumViewModel?>();
        ShopButton = ReactiveCommand.Create(() =>
        {
            var dialog = new Views.ShopView();
            var CurrentTheme = styles.CurrentTheme;
            var viewModel = new ShopViewModel(connect, user.Username, CurrentTheme, dialog, this);
            dialog.DataContext = viewModel;
            dialog.ShowDialog(ApplicationWindow);
        });
         
    }
    // binding button
    private string entryalbumname = string.Empty;
    private string entrysongname = string.Empty;
    private string entrysongduration = string.Empty;
    public ICommand AddAlbum {get; }
    public ICommand DeleteAlbum {get; }
    public ICommand AddSong {get; }
    public ICommand DeleteSong {get; }
    public string EntryAlbumName
    {
        get
        {
            return entryalbumname;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref entryalbumname, value);
            if (EntryAlbumName.Length >= 4 && SelectedArtist != String.Empty)
            {
                EnableAdd = true;
            }
            else
            {
                EnableAdd = false;
            }
        }
    }
    public string EntrySongName
    {
        get
        {
            return entrysongname;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref entrysongname, value);
            if (entrysongname.Length >= 4)
            {
                EnableAddSong = true;
            }
            else
            {
                EnableAddSong = false;
            }
        }
    }
    public string EntrySongDuration
    {
        get
        {
            return entrysongduration;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref entrysongduration, value);
        }
    }
    private bool enableadd = false;
    public bool EnableAdd
    {
        get => enableadd;
        set => this.RaiseAndSetIfChanged(ref enableadd, value);
    }
    private bool enableaddsong = false;
    public bool EnableAddSong
    {
        get => enableaddsong;
        set => this.RaiseAndSetIfChanged(ref enableaddsong, value);
    }
    public ICommand ShopButton {get; }
    public ICommand AlbumCoverPath {get; }
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
            populateSongs(connect);
        }
    }

    int selectedsong = default!;
    public int SelectedSong
    {
        get => selectedsong;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedsong, value);
        }
    }

    public void populateSongs(ConnectDB connect)
    {
        Songs.Clear();
        connect.getSongID(Albums[SelectedAlbum],ref Songs);
        if(Songs.Count != 0)
        {
            foreach (int Song_id in Songs)
            {
                SongResults.Add(new SongViewModel(connect.getSongName(Song_id), connect.getSongAlbum(Song_id), connect.getSongDuration(Song_id)));
            }
        }
    }

    public void populateAlbums(ConnectDB connect)
    {
        Albums.Clear();
        SearchResults.Clear();
        //connect.getAllAlbumsID(ref Albums);
        if(connect.getRole(username) == "Admin")
        {
            connect.getAllAlbumsID(ref Albums);
        }else
        {
            connect.getAlbumID(connect.getId(username),ref Albums);
        }
        if(Albums.Count != 0)
        {
            foreach (int Album_id in Albums)
            {
                SearchResults.Add(new AlbumViewModel(connect.getAlbumName(Album_id), connect.getAlbumArtist(Album_id), connect.getSongCount(Album_id), connect.getAlbumCover(Album_id)));
            }
        }
    }

    public List<string> artists = default!;
    public List<string> Artists
    {
        get => artists;
        set => this.RaiseAndSetIfChanged(ref artists, value);
    }

    private String selectedartist = String.Empty;
    public String SelectedArtist
    {
        get => selectedartist;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedartist, value);
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