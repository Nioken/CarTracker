using CarTracker.Bot.Commands;
using CarTracker.Database;
using CarTracker.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarTracker.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> RegisterAsync(long chatId)
    {
        var user = new User(chatId);
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUserByIdAsync(long chatId) => await _context.Users.FirstOrDefaultAsync(u=>u.ChatId == chatId);
    
    public async Task SetUserState(long chatId, UserState state)
    {
        var user = await GetUserByIdAsync(chatId);
        
        if(user == null)
            return;
        
        user.State = state;
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserState> GetState(long chatId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.ChatId == chatId);

        if (user == null)
            return UserState.Default;
        
        return user.State;
    }
}