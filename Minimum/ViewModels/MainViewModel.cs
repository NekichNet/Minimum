
using Minimum.Models;
using Minimum.Views;
using System.Dynamic;

namespace Minimum.ViewModels;

public class MainViewModel : ViewModelBase
{
    private MainWindow _mainView;

    public MainViewModel(MainWindow mainWindow)
    {
        _mainView = mainWindow;
        (_mainView.ChatList.DataContext as ChatListViewModel).OnChoosingOption += SetSelectionContent;
    }

    public void SetSelectionContent(ChatChooserOption choosenOption)
    {
        _mainView.SelectionContainer.Child = choosenOption.AssignedUserControl;
    }
}
