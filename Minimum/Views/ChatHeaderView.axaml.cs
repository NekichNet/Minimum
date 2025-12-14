using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Minimum.ViewModels;

namespace Minimum.Views;

public partial class ChatHeaderView : UserControl
{
    public static readonly StyledProperty<string> ChatIdCustomProperty =
        AvaloniaProperty.Register<ChatHeaderView, string>(nameof(ChatId), defaultValue: "");

    public string ChatId
    {
        get => GetValue(ChatIdCustomProperty);
        set => SetValue(ChatIdCustomProperty, value);
    }

    public ChatHeaderView()
    {
        InitializeComponent();
        DataContext = new ChatHeaderViewModel();
        (DataContext as ChatHeaderViewModel).Text_ChatId = $"{ChatId}";
    }
}