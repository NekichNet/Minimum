using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;

namespace Minimum.View;

public partial class CreateChatView : UserControl
{
    public CreateChatView()
    {
        InitializeComponent();
        DataContext = new CreateChatViewModel();
    }
}