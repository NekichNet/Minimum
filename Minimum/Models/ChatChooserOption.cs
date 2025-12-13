using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Models
{
    public class ChatChooserOption
    {
        public string Name { get; set; }
        public UserControl AssignedUserControl { get; set; }
        public ChatChooserOption(string name)
        {
            Name = name;
        }
        public ChatChooserOption()
        {
            Name = "";
        }
        public ChatChooserOption(Chat chat)
        {
            Name = chat.Name;
        }
    }
}
