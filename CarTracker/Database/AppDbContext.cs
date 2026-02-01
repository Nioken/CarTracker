using CarTracker.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarTracker.Database;

public class AppDbContext : DbContext
{
    public DbSet<User?> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    
    public DbSet<Expence> Expences { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка связи Один-ко-Многим
        modelBuilder.Entity<User>()
            .HasMany(u => u.Cars)
            .WithOne(c => c.User)     
            .HasForeignKey(c => c.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Car>()
            .HasMany(c => c.Expenses)
            .WithOne(e => e.Car)     
            .HasForeignKey(e => e.CarId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.ChatId)
            .IsUnique();
    }
}