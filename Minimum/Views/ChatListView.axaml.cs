using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;

namespace Minimum.Views;

public partial class ChatListView : UserControl
{
    public ChatListView()
    {
        InitializeComponent();
        DataContext = new ChatListViewModel();
    }
}