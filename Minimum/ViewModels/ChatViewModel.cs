using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using Minimum.Models;
using Minimum.Services;
using Minimum.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        public ChatView Parent { get; set; }
        public ChatHeaderViewModel Assigned_ChatHeaderViewModel { get; set; }
        public string Input_Message { get; set; }
        public Chat ChatData { get; set; }

        public ReactiveCommand<Unit, Unit> Click_AttachFile { get; set; }
        public ReactiveCommand<Unit, Unit> Click_SendMessage { get; set; }
        public ReactiveCommand<string, Unit> DownloadFileCommand { get; }
        private CacheService _cache;
        private Message msg;
        private const int KeepLastMessages = 50;
        private readonly ServerConnectionManager _scm;

        public ChatViewModel(Chat chat, CacheService cacheService, ChatView parent)
        {
            ChatData = chat;
            Messages.AddRange(chat.Messages);
            Users.AddRange(chat.Users);
            Id = chat.Id;
            Name = chat.Name;
            Assigned_ChatHeaderViewModel = new ChatHeaderViewModel();
            Assigned_ChatHeaderViewModel.SetPictureDelegateHolder = SetBackgroundPicture;
            Click_AttachFile = ReactiveCommand.CreateFromTask(AttachFile);
            Click_SendMessage = ReactiveCommand.CreateFromTask(SendMessage);
            _cache = new CacheService();

            ChatHeaderView chatHeader = new ChatHeaderView();
            ChatHeaderViewModel chatHeaderModel = new ChatHeaderViewModel();
            chatHeaderModel.ChatData = chat;
            chatHeader.DataContext = chatHeaderModel;

            Messages.AddRange(chat.Messages);
            Users.AddRange(chat.Users);
            Id = chat.Id;
            Name = chat.Name;
            _cache = cacheService;

            _ = LoadCachedMessagesAsync(chat.Id);


            

            (parent as ChatView).ChatHeaderContainer.Child = chatHeader;

            App.ServiceProvider.GetRequiredService<ServerConnectionManager>().StartListeningAsync(this);

        }
        

        public async Task SendMessage()
        {
            if (!string.IsNullOrEmpty(Input_Message.Trim()))
            {

                App.ServiceProvider.GetRequiredService<ServerConnectionManager>().SendMessage(Input_Message, ChatData.Id);
                Messages.Add(new Message() { 
                    Author = App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser,
                    AuthorId = App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser.Id,
                    Text = Input_Message,
                    Time = DateTime.Now
                });
                await SaveMessagesToCacheAsync(ChatData.Id);
            }
        }



        public async Task AttachFile()
        {
            try
            {
                var topLevel = TopLevel.GetTopLevel(new MainWindow());

                var image = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Выберите файл для отправки",
                    AllowMultiple = false,
                    FileTypeFilter = new[]
                        {
                        new FilePickerFileType("Любой")
                        {
                            Patterns = new[] { "*" },
                        }
                    }
                });
                string filePath = (image[0].TryGetLocalPath());

                string FileContent = File.ReadAllText(filePath);

                string FileId = string.Empty;

                using (SHA256 sha256 = SHA256.Create()) {

                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()+FileContent));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    FileId = builder.ToString();
                }

                App.ServiceProvider.GetRequiredService<ServerConnectionManager>().UploadFileChunk(FileId, UTF8Encoding.UTF8.GetBytes(FileContent), true);
            }
            catch { }
        }



        public void SetBackgroundPicture(string filepath)
        {
            try
            {
                using var stream = File.OpenRead(filepath);
                ImageSource = Bitmap.DecodeToHeight(stream, 800, BitmapInterpolationMode.HighQuality);
                
            }
            catch (Exception ex)
            {
                ImageSource = null;
            }
        }

        [Reactive] public Bitmap? ImageSource { get; set; }



        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<Chat> Chats { get; } = new ObservableCollection<Chat>();
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>() { };

        



        public void AddMessage(Message msg)
        {
            Messages.Add(msg);
            _ = SaveMessagesToCacheAsync(Id);
        }



        public async Task LoadCachedChatsAsync()
        {
            var chats = await _cache.LoadChatsAsync();
            Chats.Clear();
            await SaveChatsToCacheAsync();
            foreach (var c in chats.OrderByDescending(ch => ch.Id))
            {
                Chats.Add(c);
                await SaveChatsToCacheAsync();
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



        private async Task AttachFileAsync()
        {
            var topLevel = TopLevel.GetTopLevel(Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime d ? d.MainWindow : null);
            if (topLevel == null) return;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Выберите файл",
                AllowMultiple = false
            });

            var file = files?.FirstOrDefault();
            if (file == null) return;

            var path = file.TryGetLocalPath();
            if (string.IsNullOrEmpty(path)) return;

            var fi = new FileInfo(path);
            var fileId = Guid.NewGuid().ToString();

            var resp = await _scm.SendFilePlaceholder(fi.Name, fi.Length, fileId, Id);

            if (resp.Success)
            {
                var msg = new Message(fi.Name, fi.Length, fileId, 0, Id, new User { Name = "Я" })
                {
                    IsFile = true,
                    IsUploaded = false
                };
                await AppendMessageAndCacheAsync(Id, msg);

                using var fs = File.OpenRead(path);
                var buffer = new byte[8192];
                int read;
                while ((read = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    bool isComplete = fs.Position == fs.Length;
                    var chunk = buffer.Take(read).ToArray();
                    await _scm.UploadFileChunk(fileId, chunk, isComplete);
                }

                msg.IsUploaded = true;
                await SaveMessagesToCacheAsync(Id);
            }
        }



        public string? GetCachedFilePath(string fileId) => _cache.GetFilePath(fileId);
        public bool IsFileCached(string fileId) => _cache.FileExists(fileId);
        public async Task CacheFileAsync(string fileId, byte[] data) => await _cache.SaveFileAsync(fileId, data);

        public string? GetCachedAvatarPath(string id) => _cache.GetAvatarPath(id);
        public async Task CacheAvatarAsync(string id, byte[] data) => await _cache.SaveAvatarAsync(id, data);
    }
}
