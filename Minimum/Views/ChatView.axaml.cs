using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.Models;
using Minimum.Services;
using Minimum.ViewModels;

namespace Minimum.Views;

public partial class ChatView : UserControl
{
    CacheService CacheService = new CacheService();
    public ChatView(Chat chat)
    {
        InitializeComponent();
<<<<<<< HEAD
        DataContext = new ChatViewModel(chat, CacheService);
=======
        DataContext = new ChatViewModel(chat, this);
>>>>>>> 523ba299d8f1d0dbc01aff45bef5c1d02e84c397
    }
}