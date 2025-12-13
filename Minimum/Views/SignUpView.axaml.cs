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
}