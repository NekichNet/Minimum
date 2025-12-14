using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.DependencyInjection;
using Minimum.Services;
using Minimum.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public string Name { get; set; } = "Настройки";
        public string UsernameTag { get; set; } = "Имя пользователя:";
        public string AvatarTag { get; set; } = "Аватар:";
        public string LoadAvatarTag { get; set; } = "Загрузить аватар";
        public string QuitTag { get; set; } = "Выйти из аккаунта";
        public string Username { get; set; } = string.Empty;
        [Reactive] public Bitmap? Avatar { get; set; }



        public ReactiveCommand<Unit, Unit> Click_UploadPFP { get; set; }
        public ReactiveCommand<Unit, Unit> Click_LogOff { get; set; }

        public SettingsViewModel()
        {
            Click_UploadPFP = ReactiveCommand.CreateFromTask(UploadPFP);
            Click_LogOff = ReactiveCommand.CreateFromTask(LogOff);

        }



        public async Task LogOff()
        {
            // Код для того, чтобы выйти из аккаунта и удалить токен усера
        }






        public async Task UploadPFP()
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
                string imagePath = (image[0].TryGetLocalPath());

                SetPFP(imagePath);


            }
            catch { }



        }
        private void SetPFP(string imagePath)
        {
            try
            {
                using var stream = File.OpenRead(imagePath);
                Avatar = Bitmap.DecodeToHeight(stream, 800, BitmapInterpolationMode.HighQuality);



                App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser.Avatar = Avatar;
                App.ServiceProvider.GetRequiredService<ServerConnectionManager>().UpdateUser(App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser);
            }
            catch (Exception ex)
            {
                Avatar = null;
            }
        }

    }
}