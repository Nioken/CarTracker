using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot.Callbacks;

public class DeleteCarCallback : ICallbackHandler
{
    private readonly ICarService _carService;

    public DeleteCarCallback(ICarService carService)
    {
        _carService = carService;
    }
    
    public bool CanHandle(string callbackData)
    {
        return callbackData.StartsWith("DeleteCar");
    }

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        var carId = Guid.Parse(callbackQuery.Data.Split(' ')[1]);

        await _carService.DeleteCar(carId);
        
        await bot.EditMessageText(callbackQuery.From.Id, callbackQuery.Message.Id, callbackQuery.Message.Text, replyMarkup:
            DrawButtons.MyCarsList(await _carService.GetAllUserCars(callbackQuery.From.Id)));
    }
}