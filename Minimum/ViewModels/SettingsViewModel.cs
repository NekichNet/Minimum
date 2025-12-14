using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class SettingsViewModel
    {
        public string Name { get; set; } = "Настройки";
        public string UsernameTag { get; set; } = "Имя пользователя:";
        public string AvatarTag { get; set; } = "Аватар:";
        public string LoadAvatarTag { get; set; } = "Загрузить аватар";
        public string QuitTag { get; set; } = "Выйти из аккаунта";
        public string Username { get; set; } = string.Empty;
        public Bitmap Avatar { get; set; }
    }
}