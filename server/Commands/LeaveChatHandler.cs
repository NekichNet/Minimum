using Minimum.Repositories.Interfaces;
using server.Models;
using System.Net.Sockets;

namespace server.Commands;

public class LeaveChatHandler : CommandHandler
{
    public LeaveChatHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository) : base(userRepository, chatRepository, messageRepository) { }

    public override async Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client)
    {
        var (isValid, user) = await ValidateTokenAsync(request.Token);
        if (!isValid)
        {
            return new Response { Success = false, Message = "Невалидный токен." };
        }

        if (request.ChatId == null)
        {
            return new Response { Success = false, Message = "ID чата не указан." };
        }

        var chat = await ChatRepository.GetChatByIdAsync(request.ChatId.Value);
        if (chat == null)
        {
            return new Response { Success = false, Message = "Чат не найден." };
        }

        if (!chat.Users.Contains(user))
        {
            return new Response { Success = false, Message = "Пользователь не состоит в этом чате." };
        }

        chat.Users.Remove(user);
        await ChatRepository.UpdateChatAsync(chat);

        return new Response { Success = true, Message = "Вы покинули чат." };
    }
}