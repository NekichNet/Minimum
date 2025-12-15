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
    public Chat Chat { get; set; }

    public ChatView(Chat chat)
    {
        InitializeComponent();
        Chat = chat;
        DataContext = new ChatViewModel(chat, this);
    }
}