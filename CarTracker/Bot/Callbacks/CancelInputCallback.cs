using CarTracker.Bot.Commands;
using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot.Callbacks;

public class CancelInputCallback : ICallbackHandler
{
    private readonly IUserService _userService;
    
    public CancelInputCallback(IUserService userService)
    {
        _userService = userService;
    }
    
    public bool CanHandle(string callbackData)
    {
        return callbackData.StartsWith("CancelInput");
    }

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        var chatId = callbackQuery.Message.Chat.Id;
        _userService.SetUserState(chatId, UserState.Default);
        
        await bot.EditMessageText(chatId, callbackQuery.Message.Id, $"Ввод отменен!", replyMarkup: null);
    }
}