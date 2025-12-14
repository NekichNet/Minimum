using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public int? ChatId { get; set; }
        public string? ChatName { get; set; }

        public Chat? Chat { get; set; }

        public object Data { get; set; } = null!;
    }
}
