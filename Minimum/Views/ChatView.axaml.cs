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
        DataContext = new ChatViewModel(chat, CacheService);
    }
}