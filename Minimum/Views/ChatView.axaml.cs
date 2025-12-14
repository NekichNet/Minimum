using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.Models;
using Minimum.ViewModels;

namespace Minimum.Views;

public partial class ChatView : UserControl
{
    public ChatView(Chat chat)
    {
        InitializeComponent();
        DataContext = new ChatViewModel(chat, this);
    }
}