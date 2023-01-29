using System;
using Avalonia.Controls;
using ReactiveUI;
using System.Windows.Input;


namespace Szprotify.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    public ConnectDB connect = default!;
    public ProfileViewModel(ConnectDB connect, string username, StyleManager.Theme style, Window view, ApplicationWindowViewModel app)
    { 
        var styleinshop = new StyleManager(view);
        styleinshop.UseTheme(style);

        this.connect = connect;
        entrystreet = connect.getStreet(username);
        entrycity = connect.getCity(username);
        entryzipcode = connect.getZipcode(username);

        SaveData = ReactiveCommand.Create(()=>{
            connect.saveData(username, EntryStreet, EntryCity, EntryZipcode);
            view.Close();
        });
    }
    // binding button
    public ICommand SaveData {get; }
    private String entrystreet = String.Empty;
    public String EntryStreet
    {
        get => entrystreet;
        set => this.RaiseAndSetIfChanged(ref entrystreet, value);
    }
    private String entrycity = String.Empty;
    public String EntryCity
    {
        get => entrycity;
        set => this.RaiseAndSetIfChanged(ref entrycity, value);
    }
    private String entryzipcode = String.Empty;
    public String EntryZipcode
    {
        get => entryzipcode;
        set => this.RaiseAndSetIfChanged(ref entryzipcode, value);
    }
}