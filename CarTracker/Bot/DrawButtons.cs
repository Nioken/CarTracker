using CarTracker.Database;
using CarTracker.Database.Entities;
using Telegram.Bot.Types.ReplyMarkups;

namespace CarTracker.Bot;

public static class DrawButtons
{
    public static ReplyKeyboardMarkup MainMenuButtons()
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("🧾 Профиль"),
            },
        })
        {
            ResizeKeyboard = true
        };

        return keyboard;
    }

    public static InlineKeyboardMarkup MyCars()
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🚗 Мои машины", $"GetMyCars")
            },
        });
            
        return keyboard;
    }

    public static InlineKeyboardMarkup MyCarsList(List<Car> userCars)
    {
        var lines = new List<List<InlineKeyboardButton>>();

        foreach (var car in userCars)
        {
            var buttons = new List<InlineKeyboardButton>();

            buttons.Add(InlineKeyboardButton.WithCallbackData($"{car.Name}", $"ShowCarInfo {car.Id}"));
            buttons.Add(InlineKeyboardButton.WithCallbackData("⭕ Удалить", $"DeleteCar {car.Id}"));

            lines.Add(buttons);
        }

        var AddictiveButtons = new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("➕", $"AddNewCar") };
        lines.Add(AddictiveButtons);
        
        var backButton = new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("🔙", $"BackToMainMenu") };
        lines.Add(backButton);
            
        return new InlineKeyboardMarkup(lines);
    }

    public static InlineKeyboardMarkup? AddExpense(Car car)
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🧾 Список вложений", $"ShowExp {car.Id}"),
                InlineKeyboardButton.WithCallbackData("➕ Добавить вложение", $"NewExp {car.Id}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🔙", $"BackToListCar"),
            },
        });
            
        return keyboard;
    }

    public static InlineKeyboardMarkup? ShowExp(List<Expence> expenses)
    {
        var lines = new List<List<InlineKeyboardButton>>();

        foreach (var exp in expenses)
        {
            var buttons = new List<InlineKeyboardButton>();

            buttons.Add(InlineKeyboardButton.WithCallbackData($"{exp.Name}", $"ShowExpInfo {exp.Id}"));
            buttons.Add(InlineKeyboardButton.WithCallbackData($"{exp.Amount}", $"null"));
            buttons.Add(InlineKeyboardButton.WithCallbackData("⭕ Удалить", $"DeleteExp {exp.Id}"));

            lines.Add(buttons);
        }

        var AddictiveButtons = new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("➕ Добавить вложение", $"NewExp {expenses.First().CarId}") };
        lines.Add(AddictiveButtons);
        
        var backButton = new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("🔙", $"BackToCar {expenses.First().CarId}") };
        lines.Add(backButton);
            
        return new InlineKeyboardMarkup(lines);
    }

    public static InlineKeyboardMarkup CancelInput()
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("⭕ Отмена", $"CancelInput")
            },
        });
            
        return keyboard;
    }
}