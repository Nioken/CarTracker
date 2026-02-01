using CarTracker.Bot.Commands;
using CarTracker.Database.Entities;

namespace CarTracker.Services;

public interface IUserService
{
    public Task<User?> RegisterAsync(long chatId);
    
    public Task<User?> GetUserByIdAsync(long chatId);
    
    public Task SetUserState(long chatId, UserState state);
    public Task<UserState> GetState(long chatId);
}