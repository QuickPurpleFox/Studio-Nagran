using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Controls;
using ReactiveUI;
using System.Threading.Tasks;

namespace Szprotify.Views;

public partial class ApplicationWindow : ReactiveWindow<ViewModels.ApplicationWindowViewModel>
{
    public ApplicationWindow()
    {
        InitializeComponent();
        AvaloniaXamlLoader.Load(this);
        this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }

    private async Task DoShowDialogAsync(InteractionContext<ViewModels.ShopViewModel, ViewModels.AlbumViewModel?> interaction)
        {
            var dialog = new Views.ShopView();
            dialog.DataContext = interaction.Input;

            await dialog.ShowDialog<ViewModels.AlbumViewModel?>(this);
        }
}