using Avalonia.Controls;
using Avalonia;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.DependencyInjection;
using Minimum.Services;
using Minimum.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Minimum.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Color = Avalonia.Media.Color;

namespace Minimum.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private SettingsView _settingsView;
        private Avalonia.Styling.ThemeVariant _theme;

        public string Name { get; set; } = "Настройки";
        public string UsernameTag { get; set; } = "Имя пользователя:";
        public string AvatarTag { get; set; } = "Аватар:";
        public string LoadAvatarTag { get; set; } = "Загрузить аватар";
        public string ColorTag { get; set; } = "Акцентный цвет:";
        public string UpdateColorTag { get; set; } = "Сохранить цвет";
        public string QuitTag { get; set; } = "Выйти из аккаунта";
        public string Username { get; set; } = string.Empty;
        public string ThemeTag { get; set; } = "Тема приложения:";
        public Avalonia.Styling.ThemeVariant Theme
        {
            get { return _theme; }
            set { _theme = value; Application.Current.RequestedThemeVariant = value; }
        }
        public ObservableCollection<Avalonia.Styling.ThemeVariant> Themes { get; set; } = new ObservableCollection<Avalonia.Styling.ThemeVariant>()
        {
            Avalonia.Styling.ThemeVariant.Default,
            Avalonia.Styling.ThemeVariant.Dark,
            Avalonia.Styling.ThemeVariant.Light
        };

        private void UpdateAccentColor()
        {
            Color new_color = (Color)_settingsView.AccentPicker.Color;
            Color new_color2 = new Color(
                        new_color.A,
                        Convert.ToByte((Int32)(new_color.R * 0.8)),
                        Convert.ToByte((Int32)(new_color.G * 0.8)),
                        Convert.ToByte((Int32)(new_color.B * 0.8))
                    );
            Application.Current.Resources.Clear();
            Application.Current.Resources.Add("Accent1", new_color);
            Application.Current.Resources.Add("Accent2", new_color2);
            Application.Current.Resources.Add(
                "AccentForeground",
                (new_color2.R * 0.299 + new_color2.G * 0.587 + new_color2.B * 0.114) / 255 > 0.5 ?
                Avalonia.Media.Colors.Black : Avalonia.Media.Colors.White);
        }

        public ReactiveCommand<Unit, Unit> UpdateAccent { get; set; }

        public SettingsViewModel(SettingsView settingsView)
        {
            _settingsView = settingsView;
            UpdateAccent = ReactiveCommand.Create(UpdateAccentColor);
            UpdateAccentColor();
        }
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