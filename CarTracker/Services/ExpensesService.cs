using CarTracker.Database;
using CarTracker.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarTracker.Services;

public class ExpensesService : IExpensesService
{
    private readonly AppDbContext _context;

    public ExpensesService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Expence> GetExpenseAsync(int id) => await _context.Expences.FirstOrDefaultAsync(e=>e.Id == id);

    public async Task<List<Expence>> GetAllExpensesAsync(Guid carId) => await _context.Expences.Where(exp => exp.CarId == carId).ToListAsync();
    
    public async Task<double> GetTotalInvestmentAsync(Guid carId)
    {
        return await _context.Expences
            .Where(e => e.Car.Id == carId)
            .SumAsync(e => e.Amount);
    }
    
    public async Task<double> GetTotalInvestmentAsync(long chatId)
    {
        return await _context.Expences
            .Where(e => e.Car.User.ChatId == chatId)
            .SumAsync(e => e.Amount);
    }

    public async Task AddExpenseAsync(Guid carId, double amount, string name)
    {
        var Expence = new Expence(carId, amount, name);
        
        await _context.Expences.AddAsync(Expence);
        await _context.SaveChangesAsync();
    }
}