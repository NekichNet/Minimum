using DynamicData;
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
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>()
        {
            new Message{Text = "Сообщениe", Time = DateTime.Now.AddMinutes(-5)},
            new Message{Text = "Сообщениe1", Time = DateTime.Now.AddMinutes(-4)},
            new Message{Text = "Сообщениe2", Time = DateTime.Now.AddMinutes(-3)},
            new Message{Text = "Сообщениe3", Time = DateTime.Now.AddMinutes(-2)},
            new Message{Text = "Сообщениe5", Time = DateTime.Now.AddMinutes(-1)}
        };

        public ChatViewModel(Chat chat)
        {
            Messages.AddRange(chat.Messages);
            Users.AddRange(chat.Users);
            Id = chat.Id;
            Name = chat.Name;
        }
    }
}
