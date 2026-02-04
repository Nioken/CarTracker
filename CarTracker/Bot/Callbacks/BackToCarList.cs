using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot.Callbacks;

public class BackToCarList : ICallbackHandler
{
    private readonly ICarService _carService;
    private readonly IExpensesService _expensesService;

    public BackToCarList(ICarService carService, IExpensesService expensesService)
    {
        _carService = carService;
        _expensesService = expensesService;
    }
    
    public bool CanHandle(string callbackData)
    {
        return callbackData == "BackToListCar";
    }

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        await bot.EditMessageText(callbackQuery.From.Id, callbackQuery.Message.Id, $"🚗 Твои авто:", replyMarkup: DrawButtons.MyCarsList(await _carService.GetAllUserCars(callbackQuery.From.Id)));
    }
}