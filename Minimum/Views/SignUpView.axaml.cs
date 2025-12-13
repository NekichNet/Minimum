using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;

namespace Minimum;

public partial class SignUpView : UserControl
{
    public SignUpView()
    {
        InitializeComponent();
        DataContext = new SignUpViewModel();
    }

    public SignUpViewModel? ViewModel => DataContext as SignUpViewModel;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}