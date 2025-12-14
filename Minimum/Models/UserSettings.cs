using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Models
{
    public class UserSettings
    {
        public string Username { get; set; } = string.Empty;
        public string Theme { get; set; } = "Default"; // "Default", "Dark", "Light"
        public string Accent1 { get; set; } = "#FF0000"; // hex цвет
        public string Accent2 { get; set; } = "#AA0000";
        public string AccentForeground { get; set; } = "#FFFFFF";
        public string? AvatarPath { get; set; } // путь к файлу аватара
    }
}
