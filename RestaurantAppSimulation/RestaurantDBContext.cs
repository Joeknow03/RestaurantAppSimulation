using Microsoft.EntityFrameworkCore;

namespace RestaurantAppSimulation;

public class RestaurantDBContext : DbContext
{
    
    public DbSet<Session> Sessions { get; set; }
    public DbSet<CustomerOrder> CustomerOrders { get; set; }
    public DbSet<CookingResult> CookingResults { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Database=restaurant_sim;Username=jahongirakiljonov;Password="
        );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerOrder>()
            .HasOne(co => co.Session)
            .WithMany(s => s.CustomerOrders)
            .HasForeignKey(co => co.SessionId);
        
        modelBuilder.Entity<CookingResult>()
            .HasOne(cr => cr.Session)
            .WithOne(s => s.CookingResult)
            .HasForeignKey<CookingResult>(cr => cr.SessionId);
    }
}