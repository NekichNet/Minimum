using Minimum.Repositories.Interfaces;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server.Commands
{
    public class LogoutHandler : CommandHandler
    {
        public LogoutHandler(
            IUserRepository userRepository,
            IChatRepository chatRepository,
            IMessageRepository messageRepository)
            : base(userRepository, chatRepository, messageRepository) { }

        public override async Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client)
        {
            if (string.IsNullOrEmpty(request.Token))
                return new Response { Success = false, Message = "Токен не указан." };

            await UserRepository.DeleteTokenAsync(request.Token);

            return new Response { Success = true, Message = "Вы вышли из аккаунта." };
        }
    }
}
