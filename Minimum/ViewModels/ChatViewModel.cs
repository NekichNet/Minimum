using Avalonia.Controls;
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

        public ReactiveCommand<Unit, Unit> Click_AttachFile { get; set; }

        public ChatViewModel()
        {
            Assigned_ChatHeaderViewModel = new ChatHeaderViewModel();
            Assigned_ChatHeaderViewModel.SetPictureDelegateHolder = SetBackgroundPicture;
            Click_AttachFile = ReactiveCommand.CreateFromTask(AttachFile);
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
                            Patterns = new[] { "*"},
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
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>()
        {
            new Message{Text = "Сообщени", Author = new User(), Time = DateTime.Now},
            new Message{Text = "Сообщени1", Author = new User(), Time = DateTime.Now},
            new Message{Text = "Сообщени2", Author = new User(), Time = DateTime.Now},
            new Message{Text = "Сообщени3", Author = new User(), Time = DateTime.Now},
            new Message{Text = "Сообщени5", Author = new User(), Time = DateTime.Now}
        };

        public ChatViewModel(Chat chat)
        {
            Messages.AddRange(chat.Messages);
            Users.AddRange(chat.Users);
            Id = chat.Id;
            Name = chat.Name;
        }

        public void AddMessage(Message msg) => Messages.Add(msg);
    }
}
