using Avalonia.Controls;
using Minimum.ViewModels;

namespace Minimum.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(this);
    }
}