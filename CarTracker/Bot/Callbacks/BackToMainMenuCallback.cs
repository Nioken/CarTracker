using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot.Callbacks;

public class BackToMainMenuCallback : ICallbackHandler
{
    private readonly ICarService _carService;
    private readonly IExpensesService _expensesService;

    public BackToMainMenuCallback(ICarService carService, IExpensesService expensesService)
    {
        _carService = carService;
        _expensesService = expensesService;
    }
    
    public bool CanHandle(string callbackData)
    {
        return callbackData.StartsWith("BackToMainMenu");
    }

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        var chatId = callbackQuery.Message.Chat.Id;
        var cars = await _carService.GetAllUserCars(chatId);

        await bot.EditMessageText(chatId, callbackQuery.Message.Id, $"🧾 Твой профиль:\n" +
                                                                    $"🚗 Всего машин: {cars.Count}\n" +
                                                                    $"🏦 Всего вложено: {await _expensesService.GetTotalInvestmentAsync(chatId)}",
            replyMarkup: DrawButtons.MyCars());
    }
}