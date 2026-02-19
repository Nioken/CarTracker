namespace CarTracker.Services;

public interface IAuthService
{
    public string GenerateCode(long telegramId);

    public long? ValidateCode(string code);
}