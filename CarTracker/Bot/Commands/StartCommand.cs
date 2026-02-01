using CarTracker.Services;
using Telegram.Bot;

namespace CarTracker.Bot.Commands;

public class StartCommand : IBotCommand
{
    private readonly ITelegramBotClient _bot;
    private readonly IUserService _userService;
    
    public StartCommand(IUserService userService, ITelegramBotClient bot)
    {
        _bot = bot;
        _userService = userService;
    }
    
    public string Command => "/start";
    
    public async Task ExecuteAsync(long chatId, string text, UserState state)
    {
        var user = await _userService.GetUserByIdAsync(chatId);

        if (user == null)
            await _userService.RegisterAsync(chatId);

        await _bot.SendMessage(chatId, "Ты в главном меню!", replyMarkup: DrawButtons.MainMenuButtons());
    }
}