using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat?> GetChatByIdAsync(int id);
    }
}
