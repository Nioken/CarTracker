using CarTracker.Database;
using CarTracker.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarTracker.Services;

public class CarService : ICarService
{
    private readonly AppDbContext _context;

    public CarService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Car> GetCarById(Guid id) => await _context.Cars.FirstOrDefaultAsync(c=> c.Id == id);

    public Task<List<Car>> GetAllUserCars(long userId)
    {
        var cars = _context.Cars.Where(c=>c.ChatId == userId).ToList();
        
        return Task.FromResult(cars);
    }

    public async void AddNewCar(long chatId, string? inputsCarName)
    {
        var car = new Car(inputsCarName, chatId);
        
        await _context.Cars.AddAsync(car);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCar(Guid carId)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == carId);
        
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
    }
}