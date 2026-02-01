using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot.Callbacks;

public class ShowExpCallback : ICallbackHandler
{
    private readonly ICarService _carService;
    private readonly IExpensesService _expensesService;

    public ShowExpCallback(ICarService carService, IExpensesService expensesService)
    {
        _carService = carService;
        _expensesService = expensesService;
    }
    
    public bool CanHandle(string callbackData)
    {
        return callbackData.StartsWith("ShowExp");
    }

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        var carId = Guid.Parse(callbackQuery.Data.Split(' ')[1]);
        
        var car = await _carService.GetCarById(carId);
        var expenses = await _expensesService.GetAllExpensesAsync(carId);

        await bot.EditMessageText(callbackQuery.From.Id, callbackQuery.Message.Id,
            $"🧾 Ваши вложения в автомобиль: {car.Name}", replyMarkup: DrawButtons.ShowExp(expenses));
    }
}