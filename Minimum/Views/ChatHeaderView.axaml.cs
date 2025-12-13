using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;

namespace Minimum.Views;

public partial class ChatHeaderView : UserControl
{
    public ChatHeaderView()
    {
        InitializeComponent();
        DataContext = new ChatHeaderViewModel();
    }
}