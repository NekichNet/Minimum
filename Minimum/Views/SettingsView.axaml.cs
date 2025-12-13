using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;
using System.Collections.ObjectModel;

namespace Minimum;

public partial class SettingsView : UserControl
{
    private string _username;
    public string Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    public ObservableCollection<string> Themes { get; } =
        new ObservableCollection<string> { "Светлая", "Тёмная" };

    private string _selectedTheme;
    public string SelectedTheme
    {
        get => _selectedTheme;
        set => this.RaiseAndSetIfChanged(ref _selectedTheme, value);
    }

    public ReactiveCommand<Unit, Unit> SaveCommand { get; }

    public SettingsView()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();

        SaveCommand = ReactiveCommand.Create(() =>
        {
            System.Console.WriteLine($"Сохранили: {Username}, тема: {SelectedTheme}");
        });
    }
}