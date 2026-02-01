using System.ComponentModel.DataAnnotations;
using CarTracker.Bot.Commands;

namespace CarTracker.Database.Entities;

public class User
{
    [Key]
    public long ChatId { get; set; }
    
    public UserState State { get; set; }
    
    public ICollection<Car> Cars { get; set; } = new List<Car>();

    public User()
    {
    }

    public User(long chatId)
    {
        ChatId = chatId;
        State = UserState.Default;
    }
}