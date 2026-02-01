using System.ComponentModel.DataAnnotations;

namespace CarTracker.Database.Entities;

public class Expence
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public double Amount { get; set; }
    
    public Guid CarId { get; set; }
    
    public Car? Car { get; set; }

    public Expence()
    { }
    
    public Expence(Guid carId, double amount, string name)
    {
        CarId = carId;
        Amount = amount;
        Name = name;
    }
}