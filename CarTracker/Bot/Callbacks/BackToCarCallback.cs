using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot.Callbacks;

public class BackToCarCallback : ICallbackHandler
{
    private readonly ICarService _carService;
    private readonly IExpensesService _expensesService;

    public BackToCarCallback(ICarService carService, IExpensesService expensesService)
    {
        _carService = carService;
        _expensesService = expensesService;
    }
    
    public bool CanHandle(string callbackData)
    {
        return callbackData.StartsWith("BackToCar");
    }

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        var chatId = callbackQuery.Message.Chat.Id;
        var car = await _carService.GetCarById(Guid.Parse(callbackQuery.Data.Split(' ')[1]));
        
        await bot.EditMessageText(chatId, callbackQuery.Message.MessageId, $"🚗 {car.Name}\n\n" +
                                                                           $"🏦 Всего вложено: {await _expensesService.GetTotalInvestmentAsync(car.Id)}", replyMarkup: DrawButtons.AddExpense(car));
    }
}