using System;
using System.Reactive;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;
using System.Collections.Generic;
using Avalonia.Collections;
using Avalonia.Controls;

namespace Szprotify.ViewModels;

public class ManagementModel : ViewModelBase
{
    public ManagementModel(ConnectDB connect, string username, StyleManager.Theme style, Window view, ApplicationWindowViewModel app)
    { 
        var styleinview = new StyleManager(view);
        styleinview.UseTheme(style);
    }
    // binding button
    
}