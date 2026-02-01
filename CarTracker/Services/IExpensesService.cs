using CarTracker.Database.Entities;

namespace CarTracker.Services;

public interface IExpensesService
{
    public Task<Expence> GetExpenseAsync(int id);
    
    public Task<List<Expence>> GetAllExpensesAsync(Guid carId);
    
    public Task<double> GetTotalInvestmentAsync(Guid carId);
    
    public Task<double> GetTotalInvestmentAsync(long chatId);
    
    public Task AddExpenseAsync(Guid carId, double amount, string name);
}