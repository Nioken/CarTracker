using System.ComponentModel.DataAnnotations;

namespace CarTracker.Database.Entities;

public class Car
{
    [Key]
    public Guid Id { get; set; }

    public string Name {get; set; }
    
    public long ChatId { get; set; }
    public User? User { get; set; }
    
    public ICollection<Expence> Expenses { get; set; } = new List<Expence>();
    
    public Car(){}

    public Car(string name, long chatId)
    {
        Name = name;
        ChatId = chatId;
        
        Id = Guid.NewGuid();
    }
}