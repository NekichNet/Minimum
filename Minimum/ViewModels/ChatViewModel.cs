using Minimum.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>()
        {
            new Message{Text = "Сообщени", Time = DateTime.Now},
            new Message{Text = "Сообщени1", Time = DateTime.Now},
            new Message{Text = "Сообщени2", Time = DateTime.Now},
            new Message{Text = "Сообщени3", Time = DateTime.Now},
            new Message{Text = "Сообщени5", Time = DateTime.Now}
        };
    }
}
