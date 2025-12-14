using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
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
        public Bitmap? Avatar { get; set; } 

        [JsonIgnore]
        public List<Message> Messages { get; set; } = new List<Message>();
        [JsonIgnore]
        public List<Chat> Chats { get; set; } = new List<Chat>();

    }
}
