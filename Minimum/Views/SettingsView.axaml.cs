using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;
using System.Collections.ObjectModel;

namespace Minimum.View;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
    }
}