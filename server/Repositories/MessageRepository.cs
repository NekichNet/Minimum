using Microsoft.EntityFrameworkCore;
using Minimum.Repositories.Interfaces;
using server.Data;
using server.Models;

namespace server.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _db;

    public MessageRepository(AppDbContext db)
    {
        _db = db;
    }


    public async Task AddMessageAsync(Message message)
    {
        await _db.Messages.AddAsync(message);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteMessageAsync(int id)
    {
        var message = await _db.Messages.FindAsync(id);
        if (message != null)
        {
            _db.Messages.Remove(message);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Message?> GetMessageByIdAsync(int id)
    {
        return await _db.Messages
            .Include(m => m.Author)
            .Include(m => m.Chat)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Message?> GetMessageByFileIdAsync(string fileId)
    {
        return await _db.Messages
            .Include(m => m.Author)
            .Include(m => m.Chat)
            .FirstOrDefaultAsync(m => m.FileId == fileId);
    }

    public async Task UpdateMessageAsync(Message message)
    {
        _db.Messages.Update(message);
        await _db.SaveChangesAsync();
    }
}
