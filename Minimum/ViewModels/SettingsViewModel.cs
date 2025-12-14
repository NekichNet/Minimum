using Avalonia;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Minimum.Models;
using Minimum.Services;
using Minimum.View;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Color = Avalonia.Media.Color;

namespace Minimum.ViewModels
{
    public class SettingsViewModel
    {
        private SettingsView _settingsView;
        private Avalonia.Styling.ThemeVariant _theme;
        private readonly CacheService _cacheService;

        public string Name { get; set; } = "Настройки";
        public string UsernameTag { get; set; } = "Имя пользователя:";
        public string AvatarTag { get; set; } = "Аватар:";
        public string LoadAvatarTag { get; set; } = "Загрузить аватар";
        public string ColorTag { get; set; } = "Акцентный цвет:";
        public string UpdateColorTag { get; set; } = "Сохранить цвет";
        public string QuitTag { get; set; } = "Выйти из аккаунта";
        public string Username { get; set; } = string.Empty;
        public string ThemeTag { get; set; } = "Тема приложения:";
        public Bitmap Avatar { get; set; }
        public Avalonia.Styling.ThemeVariant Theme
        {
            get { return _theme; }
            set { _theme = value; Application.Current.RequestedThemeVariant = value;
                SaveSettings();
            }
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

            SaveSettings();
        }


        public ReactiveCommand<Unit, Unit> UpdateAccent { get; set; }

        public SettingsViewModel(SettingsView settingsView, CacheService cacheService)
        {
            _settingsView = settingsView;
            _cacheService = cacheService;

            UpdateAccent = ReactiveCommand.Create(UpdateAccentColor);
            UpdateAccentColor();

            _ = LoadSettings();
        }



        private async Task LoadSettings()
        {
            var settings = await _cacheService.LoadSettingsAsync();
            Username = settings.Username;

            Theme = settings.Theme switch
            {
                "Dark" => Avalonia.Styling.ThemeVariant.Dark,
                "Light" => Avalonia.Styling.ThemeVariant.Light,
                _ => Avalonia.Styling.ThemeVariant.Default
            };

            if (!string.IsNullOrEmpty(settings.AvatarPath) && File.Exists(settings.AvatarPath))
            {
                Avatar = new Bitmap(settings.AvatarPath);
            }

            // Восстанавливаем цвета
            Application.Current.Resources["Accent1"] = Color.Parse(settings.Accent1);
            Application.Current.Resources["Accent2"] = Color.Parse(settings.Accent2);
            Application.Current.Resources["AccentForeground"] = Color.Parse(settings.AccentForeground);
        }


        private async void SaveSettings()
        {
            var accent1 = (Color)Application.Current.Resources["Accent1"];
            var accent2 = (Color)Application.Current.Resources["Accent2"];
            var accentForeground = (Color)Application.Current.Resources["AccentForeground"];

            var settings = new UserSettings
            {
                Username = Username,
                Theme = Theme == Avalonia.Styling.ThemeVariant.Dark ? "Dark" :
                        Theme == Avalonia.Styling.ThemeVariant.Light ? "Light" : "Default",
                Accent1 = accent1.ToString(),
                Accent2 = accent2.ToString(),
                AccentForeground = accentForeground.ToString(),
                AvatarPath = "user_avatar.png"
            };

            await _cacheService.SaveSettingsAsync(settings);
        }
    }
}