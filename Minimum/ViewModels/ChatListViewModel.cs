using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using Minimum.Models;
using Minimum.Services;
using Minimum.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
                    OnChoosingOption.Invoke(Chats[_currentIndex]);  // При присвоенном OnChoosingOption вызывает его после изменения выбранного элемента
                }
            }
        }

        public ObservableCollection<ChatChooserOption> Chats { get; set; }

        public ChatListViewModel(ObservableCollection<ChatChooserOption> chats)
        {
            Chats = chats;
            LoadCachedChatsAsync();
            CurrentIndex = Chats.Count > 1? 1 : 0;
        }
        public ChatListViewModel()
        {
            Chats = new ObservableCollection<ChatChooserOption>();
            LoadCachedChatsAsync();
            CurrentIndex = 0;
        }

        public async Task LoadCachedChatsAsync()
        {
            CacheService _cache = App.ServiceProvider.GetRequiredService<CacheService>();
            var chats = await _cache.LoadChatsAsync();
            foreach (Chat c in chats.OrderByDescending(ch => ch.Id))
            {
                Chats.Add(new ChatChooserOption(new ChatView(c)));
            }
        }

        public async Task SaveChatsToCacheAsync()
        {
            CacheService _cache = App.ServiceProvider.GetRequiredService<CacheService>();
            await _cache.SaveChatsAsync(Chats.TakeLast(Chats.Count - 2).Select(chat => (chat.AssignedUserControl as ChatView).Chat));
        }
    }
}
