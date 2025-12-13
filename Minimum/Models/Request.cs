using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Models
{
    public class Request
    {
        public string Type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ChatName { get; set; }
        public int? ChatId { get; set; }
        public string MessageText { get; set; }
        public string Token { get; set; }

        public string FileName { get; set; }
        public string FileId { get; set; }
        public long FileSize { get; set; }
        public byte[] FileData { get; set; }
        public bool IsUploadComplete { get; set; }
    }
}
