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
        public ChatViewModel() { }

        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        public void AddMessage(Message msg) => Messages.Add(msg);
    }
}
