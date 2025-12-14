using Avalonia.Controls;
using Minimum.ViewModels;
using Minimum.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Models
{
    public class ChatChooserOption
    {
        public string Name { get; set; }
        public UserControl AssignedUserControl { get; set; }
        public ChatChooserOption(View.SettingsView settingsView)
        {
            Name = "Настройки";
            AssignedUserControl = settingsView;
        }
        public ChatChooserOption(View.CreateChatView settingsView)
        {
            Name = "Управление чатами";
            AssignedUserControl = settingsView;
        }
        public ChatChooserOption(ChatView chatView)
        {
            Name = (chatView.DataContext as ChatViewModel).Name;
            AssignedUserControl = chatView;
        }
    }
}
