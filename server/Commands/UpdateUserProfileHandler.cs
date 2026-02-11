using Minimum.Repositories.Interfaces;
using Newtonsoft.Json;
using server.Models;
using System.Net.Sockets;
using System.Text;

namespace server.Commands;

public class UpdateUserProfileHandler : CommandHandler
{
    private readonly string _avatarDir;

    public UpdateUserProfileHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository,
        string avatarDir) : base(userRepository, chatRepository, messageRepository)
    {
        _avatarDir = avatarDir;
        Directory.CreateDirectory(_avatarDir);
    }

    public override async Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client)
    {
        var (isValid, user) = await ValidateTokenAsync(request.Token);
        if (!isValid)
        {
            return new Response { Success = false, Message = "Невалидный токен." };
        }

        bool updated = false;

        if (!string.IsNullOrEmpty(request.NewUsername))
        {
            user.Name = request.NewUsername;
            updated = true;
        }

        if (request.AvatarData.Length > 0)
        {
            string fileExtension = Path.GetExtension(request.AvatarFileName);
            string uniqueFileName = $"{user.Id}_{Guid.NewGuid()}{fileExtension}";
            string filePath = Path.Combine(_avatarDir, uniqueFileName);

            try
            {
                await File.WriteAllBytesAsync(filePath, request.AvatarData);
                user.AvatarPath = uniqueFileName;
                updated = true;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = "Ошибка сохранения аватарки: " + ex.Message };
            }
        }

        if (!string.IsNullOrEmpty(request.Theme))
        { 
            user.Theme = request.Theme;
            updated = true;
        }

        if (!string.IsNullOrEmpty(request.Accent1))
        {
            user.Accent1 = request.Accent1;
            user.Accent2 = request.Accent2;
            user.AccentForeground = request.AccentForeground;
            updated = true;
        }


        if (updated)
        {
            await UserRepository.UpdateUserAsync(user);
        }

        return new Response { Success = true, Message = "Профиль обновлён." };
    }
}