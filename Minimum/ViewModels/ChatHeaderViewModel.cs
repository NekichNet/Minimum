using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Minimum.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class ChatHeaderViewModel : ViewModelBase
    {

        public delegate void SetPictureDelegate(string path);
        public SetPictureDelegate SetPictureDelegateHolder { get; set; }


        public ChatHeaderView Parent { get; set; } = new ChatHeaderView();
        public string HeaderTitle { get; set; } = "Template title";
        public string HeaderImageSource { get; set; } = string.Empty;
        public string BTN_CONTENT { get; set; } = "Вверх/вниз";




        public string Text_DeleteChatButton { get; set; } = "Удалить чат";
        public string Text_QuitChatButton { get; set; } = "Покинуть чат";
        public string Text_ChangeChatBGButton { get; set; } = "Изменить фон чата";



        public ReactiveCommand<Unit, Unit> Click_ChangeBGPicture { get; set; }

        public ChatHeaderViewModel()
        {
            Click_ChangeBGPicture = ReactiveCommand.CreateFromTask(ChangeBGPicture);
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
