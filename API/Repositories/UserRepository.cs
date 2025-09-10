using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UserRepository(AppDBContext context) : IUserRepository
{
    private readonly AppDBContext _context = context;
    public async Task<bool> CreateAsync(User user)
    {
        _context!.Users.Add(user);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var getUser = await _context.Users.FirstOrDefaultAsync(_ => _.Id == id);
        if (getUser != null)
        {
            _context.Users.Remove(getUser);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        return false;
    }

    public async Task<IEnumerable<User>> GetAllAsync() => await _context!.Users.ToListAsync();

    public async Task<User> GetByIdAsync(int id) => await _context!.Users
        .FirstOrDefaultAsync(_ => _.Id == id);
    public async Task<bool> UpdateAsync(User user)
    {
        var getUser = await _context.Users.FirstOrDefaultAsync(_ => _.Id == user.Id);
        if (getUser != null)
        {
            getUser.Name = user.Name;
            getUser.Email = user.Email;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        return false;
    }
}
