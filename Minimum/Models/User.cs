using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Bitmap? Avatar { get; set; } //= Bitmap.DecodeToHeight(File.OpenRead(path), 800); -- просто пример

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
