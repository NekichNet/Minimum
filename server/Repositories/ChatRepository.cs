using Microsoft.EntityFrameworkCore;
using Minimum.Repositories.Interfaces;
using server.Data;
using server.Models;

namespace server.Repositories
{
    public class ChatRepository : IChatRepository
    {
        public readonly AppDbContext _db;
        public ChatRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddChatAsync(Chat chat)
        {

            _ = _db.Chats.AddAsync(chat);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteChatAsync(int id)
        {
            var chat = await _db.Chats.FindAsync(id);
            if (chat != null)
            {
                _db.Chats.Remove(chat);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Chat?> GetChatByIdAsync(int id)
        {
            return await _db.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateChatAsync(Chat chat)
        {
            _db.Chats.Update(chat);
            await _db.SaveChangesAsync();
        }
    }
}
