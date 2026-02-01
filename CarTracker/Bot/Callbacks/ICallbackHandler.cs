using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot.Callbacks;

public interface ICallbackHandler
{
    bool CanHandle(string callbackData);
    
    Task HandleAsync(ITelegramBotClient bot, CallbackQuery callbackQuery);
}