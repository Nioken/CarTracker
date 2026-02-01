using CarTracker.Database.Entities;

namespace CarTracker.Services;

public interface ICarService
{
    public Task<Car> GetCarById(Guid id);
    
    public Task<List<Car>> GetAllUserCars(long userId);
    public void AddNewCar(long chatId, string? inputsCarName);
    public Task DeleteCar(Guid carId);
}