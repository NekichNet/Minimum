using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;

namespace Minimum;

public partial class SignInView : UserControl
{
    public SignInView()
    {
        InitializeComponent();
        DataContext = new SignInViewModel();
    }

    public SignInViewModel? ViewModel => DataContext as SignInViewModel;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}