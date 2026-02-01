using CarTracker.Bot.Commands;
using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot.Callbacks;

public class NewExpenseCallback : ICallbackHandler
{
    private readonly IUserService _userService;
    private readonly IInputService _inputService;

    public NewExpenseCallback(IUserService userService, IInputService inputService)
    {
        _userService = userService;
        _inputService = inputService;
    }
    
    public bool CanHandle(string callbackData)
    {
        return callbackData.StartsWith("NewExp");
    }

    public async Task HandleAsync(ITelegramBotClient bot, CallbackQuery callbackQuery)
    {
        var carId = Guid.Parse(callbackQuery.Data.Split(' ')[1]);
        var inputs = _inputService.GetInputValues(callbackQuery.From.Id);
        
        inputs.LastCarGuid = carId;
        
        await _userService.SetUserState(callbackQuery.From.Id, UserState.ExpNameInput);

        await bot.SendMessage(callbackQuery.From.Id, "Введи наименование нового вложения!\n\n" +
                                                     "Пример: Замена масла", replyMarkup: DrawButtons.CancelInput());
    }
}