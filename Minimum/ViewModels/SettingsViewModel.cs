using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using Minimum.Models;
using Minimum.Services;
using Minimum.View;
using Minimum.View;
using Minimum.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Avalonia.Media.Color;

namespace Minimum.ViewModels
{
    //public class SettingsViewModel : ViewModelBase
    //{
    //    private SettingsView _settingsView;
    //    private Avalonia.Styling.ThemeVariant _theme;
    //    private readonly CacheService _cacheService;
    //    private readonly ServerConnectionManager _scm;

    //    public string Name { get; set; } = "Настройки";
    //    public string UsernameTag { get; set; } = "Имя пользователя:";
    //    public string AvatarTag { get; set; } = "Аватар:";
    //    public string LoadAvatarTag { get; set; } = "Загрузить аватар";
    //    public string ColorTag { get; set; } = "Акцентный цвет:";
    //    public string UpdateColorTag { get; set; } = "Сохранить цвет";

    //    public string QuitTag { get; set; } = "Выйти из аккаунта";
    //    public ReactiveCommand<Unit, Unit> QuitCommand { get; private set; }

    //    //public string Username { get; set; } = string.Empty;



    //    public ReactiveCommand<Unit, Unit> Click_UploadPFP { get; set; }
    //    public ReactiveCommand<Unit, Unit> Click_LogOff { get; set; }

    //    public SettingsViewModel(SettingsView settingsView)
    //    {
    //        _settingsView = settingsView;
    //        _cacheService = App.ServiceProvider.GetRequiredService<CacheService>();
    //        UpdateAccent = ReactiveCommand.Create(UpdateAccentColor);
    //        UpdateAccentColor();

    //        _ = LoadSettings();

    //        LogOff();
    //    }
    //    [Reactive] public Bitmap? Avatar { get; set; }
    //    [Reactive] public string Username { get; set; }
    //    [Reactive] public ThemeVariant Theme { get; set; }
    //    [Reactive] public Color AccentColor { get; set; }


    //    public async Task LogOff()
    //    {
    //        QuitCommand = ReactiveCommand.CreateFromTask(async () =>
    //        {
    //            await App.ServiceProvider.GetRequiredService<ServerConnectionManager>().SignOut();

    //            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    //            {
    //                desktop.Shutdown();
    //            }
    //        });
    //    }


    //    public async Task UploadPFP()
    //    {
    //        try
    //        {
    //            var topLevel = TopLevel.GetTopLevel(new MainWindow());

    //            var image = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
    //            {
    //                Title = "Выберите изображение",
    //                AllowMultiple = false,
    //                FileTypeFilter = new[]
    //                    {
    //                    new FilePickerFileType("Изображение")
    //                    {
    //                        Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.webp" },
    //                        MimeTypes = new[] { "image/*" }
    //                    }
    //                }
    //            });
    //            string imagePath = (image[0].TryGetLocalPath());

    //            SetPFP(imagePath);


    //        }
    //        catch { }



    //    }
    //    private void SetPFP(string imagePath)
    //    {
    //        try
    //        {
    //            using var stream = File.OpenRead(imagePath);
    //            Avatar = Bitmap.DecodeToHeight(stream, 800, BitmapInterpolationMode.HighQuality);



    //            App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser.Avatar = Avatar;
    //            App.ServiceProvider.GetRequiredService<ServerConnectionManager>().UpdateUser(App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser);
    //        }
    //        catch (Exception ex)
    //        {
    //            Avatar = null;
    //        }
    //    }

    //    public string ThemeTag { get; set; } = "Тема приложения:";
    //    public Avalonia.Styling.ThemeVariant Theme
    //    {
    //        get { return _theme; }
    //        set
    //        {
    //            _theme = value; Application.Current.RequestedThemeVariant = value;
    //            SaveSettings();
    //        }
    //    }
    //    public ObservableCollection<Avalonia.Styling.ThemeVariant> Themes { get; set; } = new ObservableCollection<Avalonia.Styling.ThemeVariant>()
    //    {
    //        Avalonia.Styling.ThemeVariant.Default,
    //        Avalonia.Styling.ThemeVariant.Dark,
    //        Avalonia.Styling.ThemeVariant.Light
    //    };

    //    private void UpdateAccentColor()
    //    {
    //        Color new_color = (Color)_settingsView.AccentPicker.Color;
    //        Color new_color2 = new Color(
    //                    new_color.A,
    //                    Convert.ToByte((Int32)(new_color.R * 0.8)),
    //                    Convert.ToByte((Int32)(new_color.G * 0.8)),
    //                    Convert.ToByte((Int32)(new_color.B * 0.8))
    //                );
    //        Application.Current.Resources.Clear();
    //        Application.Current.Resources.Add("Accent1", new_color);
    //        Application.Current.Resources.Add("Accent2", new_color2);
    //        Application.Current.Resources.Add(
    //            "AccentForeground",
    //            (new_color2.R * 0.299 + new_color2.G * 0.587 + new_color2.B * 0.114) / 255 > 0.5 ?
    //            Avalonia.Media.Colors.Black : Avalonia.Media.Colors.White);

    //        SaveSettings();
    //    }


    //    public ReactiveCommand<Unit, Unit> UpdateAccent { get; set; }

    //    public SettingsViewModel(SettingsView settingsView, CacheService cacheService)
    //    {
    //        _settingsView = settingsView;
    //        _cacheService = cacheService;

    //        UpdateAccent = ReactiveCommand.Create(UpdateAccentColor);
    //        UpdateAccentColor();

    //        _ = LoadSettings();
    //    }



    //    private async Task LoadSettings()
    //    {
    //        var settings = await _cacheService.LoadSettingsAsync();
    //        Username = settings.Username;

    //        Theme = settings.Theme switch
    //        {
    //            "Dark" => Avalonia.Styling.ThemeVariant.Dark,
    //            "Light" => Avalonia.Styling.ThemeVariant.Light,
    //            _ => Avalonia.Styling.ThemeVariant.Default
    //        };

    //        if (!string.IsNullOrEmpty(settings.AvatarPath) && File.Exists(settings.AvatarPath))
    //        {
    //            Avatar = new Bitmap(settings.AvatarPath);
    //        }

    //        // Восстанавливаем цвета
    //        Application.Current.Resources["Accent1"] = Color.Parse(settings.Accent1);
    //        Application.Current.Resources["Accent2"] = Color.Parse(settings.Accent2);
    //        Application.Current.Resources["AccentForeground"] = Color.Parse(settings.AccentForeground);
    //    }


    //    private async void SaveSettings()
    //    {
    //        var accent1 = (Color)Application.Current.Resources["Accent1"];
    //        var accent2 = (Color)Application.Current.Resources["Accent2"];
    //        var accentForeground = (Color)Application.Current.Resources["AccentForeground"];

    //        var settings = new UserSettings
    //        {
    //            Username = Username,
    //            Theme = Theme == Avalonia.Styling.ThemeVariant.Dark ? "Dark" :
    //                    Theme == Avalonia.Styling.ThemeVariant.Light ? "Light" : "Default",
    //            Accent1 = accent1.ToString(),
    //            Accent2 = accent2.ToString(),
    //            AccentForeground = accentForeground.ToString(),
    //            AvatarPath = "user_avatar.png"
    //        };

    //        await _cacheService.SaveSettingsAsync(settings);
    //    }
    //}


    public class SettingsViewModel : ViewModelBase
    {
        private SettingsView _settingsView;
        private ThemeVariant _theme;
        private readonly CacheService _cacheService;
        private readonly ServerConnectionManager _scm;

        private string _avatarPath;

        public string Name { get; set; } = "Настройки";
        public string UsernameTag { get; set; } = "Имя пользователя:";
        public string AvatarTag { get; set; } = "Аватар:";
        public string LoadAvatarTag { get; set; } = "Загрузить аватар";
        public string ColorTag { get; set; } = "Акцентный цвет:";
        public string UpdateColorTag { get; set; } = "Сохранить цвет";
        public string ThemeTag { get; set; } = "Тема приложения:";

        public string QuitTag { get; set; } = "Выйти из аккаунта";
        public ReactiveCommand<Unit, Unit> QuitCommand { get; private set; }

        [Reactive] public Bitmap? Avatar { get; set; }
        [Reactive] public string Username { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> Click_UploadPFP { get; set; }
        public ReactiveCommand<Unit, Unit> UpdateAccent { get; set; }

        public ThemeVariant Theme
        {
            get => _theme;
            set
            {
                _theme = value;
                Application.Current.RequestedThemeVariant = value;
                SaveSettings();
            }
        }

        public ObservableCollection<ThemeVariant> Themes { get; set; } =
            new()
            {
                ThemeVariant.Default,
                ThemeVariant.Dark,
                ThemeVariant.Light
            };

        public SettingsViewModel(SettingsView settingsView)
        {
            _settingsView = settingsView;
            _cacheService = App.ServiceProvider.GetRequiredService<CacheService>();
            _scm = App.ServiceProvider.GetRequiredService<ServerConnectionManager>();

            UpdateAccent = ReactiveCommand.Create(UpdateAccentColor);
            Click_UploadPFP = ReactiveCommand.CreateFromTask(UploadPFP);

            UpdateAccentColor();

            _ = LoadSettings();

            InitLogOff();

            this.WhenAnyValue(x => x.Username)
                .Skip(1)
                .Subscribe(async _ =>
                {
                    SaveSettings();
                    await UpdateUserOnServer();
                });

            this.WhenAnyValue(x => x.Avatar)
                .Skip(1)
                .Subscribe(async _ =>
                {
                    SaveSettings();
                    await UpdateUserOnServer();
                });
        }

        private void InitLogOff()
        {
            QuitCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await _scm.SignOut();

                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.Shutdown();
                }
            });
        }

        public async Task UploadPFP()
        {
            try
            {
                var topLevel = TopLevel.GetTopLevel(_settingsView); 

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

                if (image == null || image.Count == 0)
                    return;

                string? imagePath = image[0].TryGetLocalPath();
                if (string.IsNullOrEmpty(imagePath))
                    return;

                SetPFP(imagePath);
            }
            catch
            {

            }
        }

        private void SetPFP(string imagePath)
        {
            try
            {
                using var stream = File.OpenRead(imagePath);
                Avatar = Bitmap.DecodeToHeight(stream, 800, BitmapInterpolationMode.HighQuality);

                App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser.Avatar = Avatar;
                _ = _scm.UpdateUser(App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser);

                _avatarPath = imagePath;
                SaveSettings();
            }
            catch
            {
                Avatar = null;
            }
        }

        private void UpdateAccentColor()
        {
            Color new_color = (Color)_settingsView.AccentPicker.Color;
            Color new_color2 = new Color(
                new_color.A,
                Convert.ToByte((int)(new_color.R * 0.8)),
                Convert.ToByte((int)(new_color.G * 0.8)),
                Convert.ToByte((int)(new_color.B * 0.8))
            );

            Application.Current.Resources.Clear();
            Application.Current.Resources.Add("Accent1", new_color);
            Application.Current.Resources.Add("Accent2", new_color2);
            Application.Current.Resources.Add(
                "AccentForeground",
                (new_color2.R * 0.299 + new_color2.G * 0.587 + new_color2.B * 0.114) / 255 > 0.5
                    ? Colors.Black
                    : Colors.White);

            SaveSettings();
        }

        private async Task LoadSettings()
        {
            var settings = await _cacheService.LoadSettingsAsync();
            Username = settings.Username;

            Theme = settings.Theme switch
            {
                "Dark" => ThemeVariant.Dark,
                "Light" => ThemeVariant.Light,
                _ => ThemeVariant.Default
            };

            _avatarPath = settings.AvatarPath;

            if (!string.IsNullOrEmpty(settings.AvatarPath) && File.Exists(settings.AvatarPath))
            {
                Avatar = new Bitmap(settings.AvatarPath);
            }

            if (!string.IsNullOrEmpty(settings.Accent1))
                Application.Current.Resources["Accent1"] = Color.Parse(settings.Accent1);
            if (!string.IsNullOrEmpty(settings.Accent2))
                Application.Current.Resources["Accent2"] = Color.Parse(settings.Accent2);
            if (!string.IsNullOrEmpty(settings.AccentForeground))
                Application.Current.Resources["AccentForeground"] = Color.Parse(settings.AccentForeground);
        }

        private async void SaveSettings()
        {
            // NEW: корректное сохранение аватара
            string avatarPath = _avatarPath;

            if (Avatar != null)
            {
                Directory.CreateDirectory("avatars");
                avatarPath = Path.Combine("avatars", "user_avatar.png");
                using var fs = File.Create(avatarPath);
                Avatar.Save(fs);
            }

            var accent1 = Application.Current.Resources.ContainsKey("Accent1")
                ? (Color)Application.Current.Resources["Accent1"]
                : Colors.LightBlue;

            var accent2 = Application.Current.Resources.ContainsKey("Accent2")
                ? (Color)Application.Current.Resources["Accent2"]
                : accent1;

            var accentForeground = Application.Current.Resources.ContainsKey("AccentForeground")
                ? (Color)Application.Current.Resources["AccentForeground"]
                : Colors.Black;

            var settings = new UserSettings
            {
                Username = Username,
                Theme = Theme == ThemeVariant.Dark ? "Dark" :
                        Theme == ThemeVariant.Light ? "Light" : "Default",
                Accent1 = accent1.ToString(),
                Accent2 = accent2.ToString(),
                AccentForeground = accentForeground.ToString(),
                AvatarPath = avatarPath
            };

            await _cacheService.SaveSettingsAsync(settings);
        }

        private async Task UpdateUserOnServer()
        {
            var user = App.ServiceProvider.GetRequiredService<UserProviderService>().CurrentUser;
            user.Name = Username;
            user.Avatar = Avatar;

            await _scm.UpdateUser(user);
        }
    }
}