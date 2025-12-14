using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Models
{
    public class BroadcastMessage
    {
        public string type { get; set; }
        public int id { get; set; }
        public string text { get; set; }
        public string author { get; set; }
        public DateTime time { get; set; }
        public bool isFile { get; set; }
        public string fileName { get; set; }
        public string fileId { get; set; }
        public bool isUploaded { get; set; }
    }
}
