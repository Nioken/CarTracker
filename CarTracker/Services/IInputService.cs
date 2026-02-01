using CarTracker.Bot;

namespace CarTracker.Services;

public interface IInputService
{
    public InputValues GetInputValues(long userId); 
    
    public Task SendInput(long chatId, InputValues inputValues);
}

public class InputValues
{
    public string CarName { get; set; }
    public string? NewExpName { get; set; }
    
    public Guid? LastCarGuid { get; set; }
    
    public double? NewExpAmount { get; set; }
}