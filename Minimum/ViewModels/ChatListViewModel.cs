using Minimum.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class ChatListViewModel : ViewModelBase
    {
        /// <summary>
        /// Делегат, который вызывается при измении выбранного индекса ListBox
        /// </summary>
        /// <returns></returns>
        public delegate void OnChoosingOptionDelegate(ChatChooserOption choosenOption);

        /// <summary>
        /// Переменная хранящая делегат, который вызывается при измении выбранного индекса ListBox
        /// </summary>
        /// <returns></returns>
        public OnChoosingOptionDelegate OnChoosingOption { get; set; } // Делегируй этой делегате метод, который будет высасывать из ChatChooserOption Вью модель выбранного чата и усё.


        private int _currentIndex = 1; // Показывает индекс выбранного в данный момент поля
        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                _currentIndex = value;
                if (OnChoosingOption != null)
                {
                    OnChoosingOption.Invoke(_chats[_currentIndex]);  // При присвоенном OnChoosingOption вызывает его после изменения выбранного элемента
                }
            }
        }

        public ChatChooserOption SettingOption { get; set; } = new ChatChooserOption("Настройки");

        private ObservableCollection<ChatChooserOption> _chats;
        public ObservableCollection<ChatChooserOption> Chats
        {
            get
            {
                return _chats;
            }
            set
            {
                _chats = value;
                _chats.Insert(0, SettingOption);
            }
        }


        public ChatListViewModel(ObservableCollection<ChatChooserOption> chats)
        {
            Chats = chats;
            CurrentIndex = 1;
        }
        public ChatListViewModel()
        {
            Chats = new ObservableCollection<ChatChooserOption>();
            /*
            {
                new ChatChooserOption(){Name = "Чатикс"},
                new ChatChooserOption(){Name = "Чатикс1"},
                new ChatChooserOption(){Name = "Чатикс2"}
            };
            */
            CurrentIndex = 0;
        }

    }
}
