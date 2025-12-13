using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message?> GetMessageByIdAsync(int id);
        Task AddMessageAsync(Message message);
        Task DeleteMessageAsync(int id);
    }
}
