using server.Models;

namespace Minimum.Repositories.Interfaces;

public interface IMessageRepository
{
    Task<Message?> GetMessageByIdAsync(int id);
    Task AddMessageAsync(Message message);
    Task DeleteMessageAsync(int id);
    Task<Message?> GetMessageByFileIdAsync(string fileId);
    Task UpdateMessageAsync(Message message);
}
