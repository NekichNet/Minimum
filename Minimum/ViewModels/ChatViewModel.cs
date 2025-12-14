using Avalonia.Media.Imaging;
using Minimum.Models;
using Minimum.Views;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        public ChatView Parent { get; set; }
        public ChatHeaderViewModel Assigned_ChatHeaderViewModel { get; set; }

        public ChatViewModel()
        {
            Assigned_ChatHeaderViewModel = new ChatHeaderViewModel();
            Assigned_ChatHeaderViewModel.SetPictureDelegateHolder = SetBackgroundPicture;
        }

        public void SetBackgroundPicture(string filepath)
        {
            try
            {
                using var stream = File.OpenRead(filepath);
                ImageSource = Bitmap.DecodeToHeight(stream, 800, BitmapInterpolationMode.HighQuality);
                
            }
            catch (Exception ex)
            {
                ImageSource = null;
            }
        }

        [Reactive] public Bitmap? ImageSource { get; set; }



        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>()
        {
            new Message{Text = "Сообщени", Author = new User(), Time = DateTime.Now},
            new Message{Text = "Сообщени1", Author = new User(), Time = DateTime.Now},
            new Message{Text = "Сообщени2", Author = new User(), Time = DateTime.Now},
            new Message{Text = "Сообщени3", Author = new User(), Time = DateTime.Now},
            new Message{Text = "Сообщени5", Author = new User(), Time = DateTime.Now}
        };
    }
}
