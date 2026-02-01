using System.Collections.Concurrent;
using CarTracker.Bot.Callbacks;
using CarTracker.Bot.Commands;
using CarTracker.Database.Entities;
using CarTracker.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CarTracker.Bot;

public class UpdateHandler
{
    private readonly IEnumerable<IBotCommand> _commands;
    private readonly IEnumerable<ICallbackHandler> _callbackHandlers;
    private readonly ITelegramBotClient _bot;
    private readonly IUserService _userService;
    private readonly ICarService _carService;
    private readonly IInputService _inputService;
    private readonly IExpensesService _expensesService;
    
    public UpdateHandler(IEnumerable<IBotCommand> commands, IEnumerable<ICallbackHandler> callbackHandlers, ITelegramBotClient bot, IUserService userService, ICarService carService, IInputService inputService, IExpensesService expensesService)
    {
        _commands = commands;
        _bot = bot;
        _userService = userService;
        _carService = carService;
        _callbackHandlers = callbackHandlers;
        _inputService = inputService;
        _expensesService = expensesService;
    }

    public async Task HandleAsync(Update update)
    {
        var chatId = update.CallbackQuery != null ? update.CallbackQuery.Message.Chat.Id : update.Message.Chat.Id;
        var state = await _userService.GetState(chatId);

        if (state != UserState.Default)
        {
            await HandleDialogAsync(chatId, update, state);
            return;
        }
        
        if (update.CallbackQuery != null)
        {
            await HandleCallbackAsync(update.CallbackQuery);
            return;
        }
        
        var text = update.Message.Text;
        
        var command = _commands.FirstOrDefault(c => text.StartsWith(c.Command));

        if (command != null)
        {
            await command.ExecuteAsync(chatId, text, state);
        }
    }

    private async Task HandleDialogAsync(long chatId, Update update, UserState state)
    {
        
        var inputs = _inputService.GetInputValues(chatId);
        
        switch (state)
        {
            case UserState.CarNameInput:
                
                inputs.CarName = update.Message.Text;
                
                _carService.AddNewCar(chatId, inputs.CarName);

                await _bot.SendMessage(chatId, $"Машина {inputs.CarName} успешно добавлена!");
                
                await _userService.SetUserState(chatId, UserState.Default);
                
                return;
            
            case UserState.ExpNameInput:
                
                inputs.NewExpName = update.Message.Text;
                
                await _userService.SetUserState(chatId, UserState.ExpAmountInput);

                await _bot.SendMessage(chatId, "Теперь введи сумму\n\n" +
                                               "Пример: (25.6)");
                
                return;
            
            case UserState.ExpAmountInput:
                
                double.TryParse(update.Message.Text, out var amount);

                if (amount <= 0)
                {
                    await _bot.SendMessage(chatId, $"Число введено не корректно!");
                    
                    return;
                }
                
                await _expensesService.AddExpenseAsync(inputs.LastCarGuid.Value, amount, inputs.NewExpName);

                _userService.SetUserState(chatId, UserState.Default);
                
                await _bot.SendMessage(chatId, "Новое вложение успешно добавлено!");
                
                return;
        }
    }

    private async Task HandleCallbackAsync(CallbackQuery callbackQuery)
    {
        var data = callbackQuery.Data;
        if (string.IsNullOrEmpty(data)) return;
        
        var handler = _callbackHandlers.FirstOrDefault(h => h.CanHandle(data));

        if (handler != null)
        {
            await handler.HandleAsync(_bot, callbackQuery);
        }
        else 
        {
            await _bot.AnswerCallbackQuery(callbackQuery.Id);
        }
    }
}