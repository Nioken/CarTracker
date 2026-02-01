namespace CarTracker.Bot.Commands;

public interface IBotCommand
{
    string Command { get; }
    Task ExecuteAsync(long chatId, string text, UserState state);
}

public enum UserState
{
    Default,
    CarNameInput,
    ExpNameInput,
    ExpAmountInput
}