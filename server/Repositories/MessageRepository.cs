using Microsoft.EntityFrameworkCore;
using Minimum.Repositories.Interfaces;
using server.Data;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _db;

        public MessageRepository(AppDbContext db)
        {
            _db = db;
        }


        public async Task AddMessageAsync(Message message)
        {
            _ = _db.Messages.AddAsync(message);
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
    }
}
