
using Minimum.Models;
using Minimum.Views;
using System.Dynamic;

namespace Minimum.ViewModels;

public class MainViewModel : ViewModelBase
{
    private MainWindow _mainView;
    public ChatChooserOption SettingOption { get; set; } = new ChatChooserOption(new View.SettingsView());
    public MainViewModel(MainWindow mainWindow)
    {
        _mainView = mainWindow;
        (_mainView.ChatList.DataContext as ChatListViewModel).OnChoosingOption += SetSelectionContent;
        (_mainView.ChatList.DataContext as ChatListViewModel).Chats.Insert(0, SettingOption);
        SetSelectionContent(SettingOption);
    }

    public void SetSelectionContent(ChatChooserOption choosenOption)
    {
        _mainView.SelectionContainer.Child = choosenOption.AssignedUserControl;
    }
}
