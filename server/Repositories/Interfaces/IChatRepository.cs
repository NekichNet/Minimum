using server.Models;

namespace Minimum.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat?> GetChatByIdAsync(int id);
        Task AddChatAsync(Chat chat);
        Task UpdateChatAsync(Chat chat);
        Task DeleteChatAsync(int id);

        Task<IEnumerable<Chat>> GetUserChatsAsync(int userId);
    }
}
