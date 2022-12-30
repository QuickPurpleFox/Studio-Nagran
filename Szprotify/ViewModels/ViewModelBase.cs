using ReactiveUI;

namespace Szprotify.ViewModels;

/*
*This is a base class that all ViewModels should inherit. 
*It implements the way the ViewModels signal changes to the Views to update the UI. 
*(google INotifyPropertyChanged for more on that.)
*/

public class ViewModelBase : ReactiveObject
{
}
