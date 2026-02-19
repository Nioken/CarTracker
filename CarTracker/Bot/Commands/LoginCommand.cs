using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CarTracker.Bot.Commands;

public class LoginCommand : IBotCommand
{
    private readonly IAuthService _authService;
    private readonly ITelegramBotClient _bot;
    
    public LoginCommand(IAuthService authService, ITelegramBotClient bot)
    {
        _bot = bot;
        _authService = authService;
    }
    
    public string Command => "/login";
    public async Task ExecuteAsync(long chatId, string text, UserState state)
    {
        var code = _authService.GenerateCode(chatId);

        await _bot.SendMessage(chatId, $"🔐 Ваш код для входа: `{code}`", ParseMode.Markdown);
    }
}