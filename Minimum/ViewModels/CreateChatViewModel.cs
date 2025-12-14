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
        public string Text_TabJoinChat { get; set; } = "Присоедениться";
        public string Text_TabCreateChat { get; set; } = "Создать";
        public string Text_CreateChat { get; set; } = "Создать чат";
        public string Text_CreateChatTitle { get; set; } = "Создание чата";
        public string Text_CreateChatChatName { get; set; } = "Введите название чата:";
        public string Input_CreateChatChatName { get; set; } = string.Empty;



        public ReactiveCommand<Unit, Unit> Click_EnterChat { get; set; }
        public string Text_JoinChat { get; set; } = "Присоедениться";
        public string Text_JoinChatTitle { get; set; } = "Присоедениться к чату";
        public string Text_JoinChatChatName { get; set; } = "Введите ID чата:";
        public string Input_JoinChatChatName { get; set; } = string.Empty;



        public CreateChatViewModel()
        {
            Click_CreateChat = ReactiveCommand.CreateFromTask(CreateChat);
            Click_EnterChat = ReactiveCommand.CreateFromTask(EnterChat);
        }

        public async Task CreateChat()
        {
            // код для создания чата при нажатии кнопки
        }
        public async Task EnterChat()
        {
            // код для создания чата при нажатии кнопки
        }
    }
}
