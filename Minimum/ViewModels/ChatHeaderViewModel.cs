using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
using Minimum.Models;
using Minimum.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Minimum.ViewModels
{
    public class ChatHeaderViewModel : ViewModelBase
    {

        public delegate void SetPictureDelegate(string path);
        public SetPictureDelegate SetPictureDelegateHolder { get; set; }




        public Chat ChatData { get; set; }

        public ChatHeaderView Parent { get; set; } = new ChatHeaderView();
        public string HeaderTitle { get; set; } = "Template title";
        public bool IsPaneWithUsersIsOpen { get; set; } = false;
        public string HeaderImageSource { get; set; } = string.Empty;
        public string BTN_CONTENT { get; set; } = "Вверх/вниз";

        public string Text_ChatId { get; set; } = "";




        public string Text_DeleteChatButton { get; set; } = "Удалить чат";
        public string Text_QuitChatButton { get; set; } = "Покинуть чат";
        public string Text_ChangeChatBGButton { get; set; } = "Изменить фон чата";



        public ReactiveCommand<Unit, Unit> Click_ChangeBGPicture { get; set; }
        public ReactiveCommand<Unit, Unit> Click_OpenUsersPane { get; set; }
        public ReactiveCommand<Unit, Unit> Click_CopyId { get; set; }
        public ReactiveCommand<Unit, Unit> Click_LeaveChat { get; set; }
        public ReactiveCommand<Unit, Unit> Click_DeleteChat { get; set; }

        public ChatHeaderViewModel()
        {
            Click_ChangeBGPicture = ReactiveCommand.CreateFromTask(ChangeBGPicture);
            Click_OpenUsersPane = ReactiveCommand.Create(() => { IsPaneWithUsersIsOpen = !IsPaneWithUsersIsOpen; });
            Click_CopyId = ReactiveCommand.CreateFromTask(CopyToClipboard);
        }


        public async Task CopyToClipboard()
        {
            var topLevel = TopLevel.GetTopLevel(new MainWindow()); // 'this' is your Window or UserControl
            var clipboard = topLevel?.Clipboard;

            if (clipboard != null)
            {
                await clipboard.SetTextAsync($"{ChatData.Id}");
            }
        }


        public async Task ChangeBGPicture()
        {



            try
            {


            var topLevel = TopLevel.GetTopLevel(new MainWindow());

            
            var image = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Выберите изображение",
                AllowMultiple = false,
                FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Изображение")
                        {
                            Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.webp" },
                            MimeTypes = new[] { "image/*" }
                        }
                    }
            });
            SetPictureDelegateHolder.Invoke(image[0].TryGetLocalPath());
            }
            catch { }



        }

    }
}
