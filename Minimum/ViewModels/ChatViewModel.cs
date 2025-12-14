using DynamicData;
using Minimum.Models;
using Minimum.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private readonly CacheService _cache;
        private const int KeepLastMessages = 50;

        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<Chat> Chats { get; } = new ObservableCollection<Chat>();
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>()
        {
            new Message{Text = "Сообщениe", Time = DateTime.Now.AddMinutes(-5)},
            new Message{Text = "Сообщениe1", Time = DateTime.Now.AddMinutes(-4)},
            new Message{Text = "Сообщениe2", Time = DateTime.Now.AddMinutes(-3)},
            new Message{Text = "Сообщениe3", Time = DateTime.Now.AddMinutes(-2)},
            new Message{Text = "Сообщениe5", Time = DateTime.Now.AddMinutes(-1)}
        };

        public ChatViewModel(Chat chat)
        {
            Messages.AddRange(chat.Messages);
            Users.AddRange(chat.Users);
            Id = chat.Id;
            Name = chat.Name;
        }


        public ChatViewModel()
        {
            _cache = new CacheService();
        }

        public void AddMessage(Message msg) => Messages.Add(msg);



        public async Task LoadCachedChatsAsync()
        {
            var chats = await _cache.LoadChatsAsync();
            Chats.Clear();
            foreach (var c in chats.OrderByDescending(ch => ch.Id))
            {
                Chats.Add(c);
            }
        }


        public async Task SaveChatsToCacheAsync()
        {
            await _cache.SaveChatsAsync(Chats.ToList());
        }



        public async Task LoadCachedMessagesAsync(int chatId)
        {
            var msgs = await _cache.LoadMessagesAsync(chatId);
            var last = msgs.OrderBy(m => m.Time).TakeLast(KeepLastMessages).ToList();
            Messages.Clear();
            foreach (var m in last)
            {
                Messages.Add(m);
            }
        }


        public async Task SaveMessagesToCacheAsync(int chatId)
        {
            await _cache.SaveMessagesAsync(chatId, Messages.ToList());
        }



        public async Task AppendMessageAndCacheAsync(int chatId, Message msg)
        {
            Messages.Add(msg);

            var cached = await _cache.LoadMessagesAsync(chatId);
            cached.Add(msg);
            var trimmed = cached.OrderBy(m => m.Time).TakeLast(KeepLastMessages).ToList();
            await _cache.SaveMessagesAsync(chatId, trimmed);
        }



        public string? GetCachedFilePath(string fileId) => _cache.GetFilePath(fileId);
        public bool IsFileCached(string fileId) => _cache.FileExists(fileId);
        public async Task CacheFileAsync(string fileId, byte[] data) => await _cache.SaveFileAsync(fileId, data);

        public string? GetCachedAvatarPath(string id) => _cache.GetAvatarPath(id);
        public async Task CacheAvatarAsync(string id, byte[] data) => await _cache.SaveAvatarAsync(id, data);
    }
}
