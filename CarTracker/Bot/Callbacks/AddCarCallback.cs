using CarTracker.Bot.Commands;
using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot.Callbacks;

public class AddCarCallback : ICallbackHandler
{
    private readonly IUserService _userService;

    public AddCarCallback(IUserService userService)
    {
        _userService = userService;
    }
    
    public bool CanHandle(string callbackData)
    {
        return callbackData.StartsWith("AddNewCar");
    }

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        await _userService.SetUserState(callbackQuery.From.Id, UserState.CarNameInput);

        await bot.SendMessage(callbackQuery.From.Id, "Введи название для новой машины:");
    }
}