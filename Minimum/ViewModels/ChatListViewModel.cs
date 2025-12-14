using Minimum.Models;
using Minimum.Views;
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

        private ObservableCollection<ChatChooserOption> _chats;
        public ObservableCollection<ChatChooserOption> Chats { get; set; }

        public ChatListViewModel(ObservableCollection<ChatChooserOption> chats)
        {
            Chats = chats;
            CurrentIndex = Chats.Count > 1? 1 : 0;
        }
        public ChatListViewModel()
        {
            Chats = new ObservableCollection<ChatChooserOption>();
            CurrentIndex = 0;
        }
    }
}
