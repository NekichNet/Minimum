using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Minimum.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AvatarPath { get; set; } = string.Empty;
        [NotMapped]
        public Bitmap? Avatar { get; set; }

        public string Theme { get; set; }
        public string Accent1 { get; set; }
        public string Accent2 { get; set; }
        public string AccentForeground { get; set; }


        [JsonIgnore]
        public List<Message> Messages { get; set; } = new List<Message>();
        [JsonIgnore]
        public List<Chat> Chats { get; set; } = new List<Chat>();

    }
}
