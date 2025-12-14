using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.Models;
using Minimum.Services;
using Minimum.ViewModels;
using System.Threading.Tasks;

namespace Minimum.Views;

public partial class ChatView : UserControl
{
    private readonly CacheService _cache = new CacheService();
    private readonly ServerConnectionManager _scm;
    public ChatView(Chat chat)
    {
        _scm = new ServerConnectionManager(_cache);
        var chatVm = new ChatViewModel(chat, _cache);

        InitializeComponent();
        DataContext = chatVm;
    }
}