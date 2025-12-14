using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.ViewModels
{
    public class CreateChatViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> Click_CreateChat { get; set; }
        public string Text_CreateChat { get; set; } = "Создать чат";
        public string Text_Title { get; set; } = "Создание чата";
        public string Input_Username { get; set; } = string.Empty;

        public CreateChatViewModel()
        {
            Click_CreateChat = ReactiveCommand.CreateFromTask(CreateChat);
        }

        public async Task CreateChat()
        {
            // код для создания чата при нажатии кнопки
        }
    }
}
