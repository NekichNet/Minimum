using Microsoft.EntityFrameworkCore;
using Minimum.Repositories.Interfaces;
using server.Data;
using server.Models;
using System.Xml.Linq;

namespace server.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user != null)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _db.Users.Include(u => u.Messages).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateUserAsync(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }

    public async Task<User?> GetUserByNameAsync(string name)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Name == name);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task AddTokenAsync(AuthToken token)
    {
        await _db.AuthTokens.AddAsync(token);
        await _db.SaveChangesAsync();
    }

    public async Task<AuthToken?> GetTokenByValueAsync(string token)
    {
        return await _db.AuthTokens
            .Include(at => at.User)
            .FirstOrDefaultAsync(at => at.Token == token);
    }

    public async Task DeleteTokenAsync(string token)
    {
        var tokenEntity = await _db.AuthTokens.FirstOrDefaultAsync(at => at.Token == token);
        if (tokenEntity != null)
        {
            _db.AuthTokens.Remove(tokenEntity);
            await _db.SaveChangesAsync();
        }
    }
}
