using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Minimum.Models;
using Minimum.Services;
using Minimum.ViewModels;
using System.Threading.Tasks;

namespace Minimum.Views;

public partial class ChatView : UserControl
{
    private readonly CacheService _cache = new CacheService();
    private readonly ServerConnectionManager _scm;

    public Chat Chat { get; set; }

    public ChatView(Chat chat)
    {
        _scm = App.ServiceProvider.GetRequiredService<ServerConnectionManager>();
        _cache = App.ServiceProvider.GetRequiredService<CacheService>();

        var chatVm = new ChatViewModel(chat, _cache, this);

        InitializeComponent();
        Chat = chat;
        DataContext = chatVm;

        _ = InitConnection(chatVm);
    }

    private async Task InitConnection(ChatViewModel chatVm)
    {
        _scm.StartListening(chatVm);
    }
}