using Avalonia.Controls;
using Minimum.ViewModels;

namespace Minimum.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var chatListVm = new ChatListViewModel(); 
        ChatList.DataContext = chatListVm; 

        DataContext = new MainViewModel(this, chatListVm);

        _ = chatListVm.LoadCachedChatsAsync();
    }
}