using CarTracker.Services;
using Telegram.Bot;

namespace CarTracker.Bot.Commands;

public class ProfileCommand : IBotCommand
{
    private readonly ITelegramBotClient _bot;
    private readonly ICarService _carService;
    private readonly IExpensesService _expensesService;
    
    public ProfileCommand(ITelegramBotClient bot, ICarService carService, IExpensesService expensesService)
    {
        _bot = bot;
        _carService = carService;
        _expensesService = expensesService;
    }
    
    public string Command => "🧾 Профиль";
    public async Task ExecuteAsync(long chatId, string text, UserState state)
    {
        var cars = await _carService.GetAllUserCars(chatId);

        await _bot.SendMessage(chatId, $"🧾 Твой профиль:\n" +
                                       $"🚗 Всего машин: {cars.Count}\n" +
                                       $"🏦 Всего вложено: {await _expensesService.GetTotalInvestmentAsync(chatId)}", replyMarkup: DrawButtons.MyCars());
    }
}