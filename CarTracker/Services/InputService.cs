using System.Collections.Concurrent;

namespace CarTracker.Services;

public class InputService : IInputService
{
    private ConcurrentDictionary<long, InputValues> _userInputs = new();
    
    public InputValues GetInputValues(long userId) => _userInputs.GetOrAdd(userId, _ => new InputValues());

    public Task SendInput(long chatId, InputValues inputValues)
    {
        if(!_userInputs.TryAdd(chatId, inputValues))
            _userInputs[chatId] = inputValues;
        
        return Task.CompletedTask;
    }
}