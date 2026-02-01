using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot.Callbacks;

public class MyCarsCallback : ICallbackHandler
{
    private readonly ICarService _carService;

    public MyCarsCallback(ICarService carService)
    {
        _carService = carService;
    }
    
    public bool CanHandle(string callbackData)
    {
        return callbackData.StartsWith("GetMyCars");
    }

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        await bot.EditMessageText(callbackQuery.From.Id, callbackQuery.Message.Id, $"🚗 Твои авто:", replyMarkup: DrawButtons.MyCarsList(await _carService.GetAllUserCars(callbackQuery.From.Id)));
    }
}