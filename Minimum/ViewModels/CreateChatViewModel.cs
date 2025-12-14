using Microsoft.Extensions.DependencyInjection;
using Minimum.Models;
using Minimum.Services;
using Minimum.Views;
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
        public ChatListViewModel ChatListViewModel { get; set; }

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
            Response response = await App.ServiceProvider.GetRequiredService<ServerConnectionManager>().CreateChat(Input_CreateChatChatName);
            if (response.Success)
            {
                Chat chat = new Chat() { Id = (int)response.ChatId, Name = Input_CreateChatChatName };
                ChatView chatView = new ChatView(chat);
                ChatChooserOption chatOption = new ChatChooserOption(chatView);
                ChatListViewModel.Chats.Add(chatOption);
                ChatListViewModel.SaveChatsToCacheAsync();
            }
        }
        public async Task EnterChat()
        {
            Response response = await App.ServiceProvider.GetRequiredService<ServerConnectionManager>().JoinChat(Convert.ToInt32(Input_JoinChatChatName));
            if (response.Success)
            {
                Chat chat = new Chat() { Id = Convert.ToInt32(Input_JoinChatChatName), Name = response.Message };
                ChatView chatView = new ChatView(chat);
                ChatChooserOption chatOption = new ChatChooserOption(chatView);
                ChatListViewModel.Chats.Add(chatOption);
                ChatListViewModel.SaveChatsToCacheAsync();
            }
        }
    }
}
