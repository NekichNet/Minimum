using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

        public Chat Chat { get; set; } = null!;
        public User? User { get; set; }
    }
}
