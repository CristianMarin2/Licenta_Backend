using Microsoft.EntityFrameworkCore;
using SelfCheckoutSystem.Models;

namespace SelfCheckoutSystem.Data;

public class SelfCheckoutDbContext : DbContext
{
    public SelfCheckoutDbContext(DbContextOptions<SelfCheckoutDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<SaleSession> SaleSessions { get; set; }
    public DbSet<SaleTransaction> SaleTransactions { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>()
            .HasKey(u => u.EmployeeCode);

        modelBuilder.Entity<User>()
            .HasMany(u => u.SaleSessions)
            .WithOne(s => s.Cashier)
            .HasForeignKey(s => s.CashierCode)
            .OnDelete(DeleteBehavior.Restrict);

        // Product
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Barcode);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.SaleItems)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductBarcode)
            .OnDelete(DeleteBehavior.Restrict);

        // SaleSession
        modelBuilder.Entity<SaleSession>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<SaleSession>()
            .HasMany(s => s.Transactions)
            .WithOne(t => t.SaleSession)
            .HasForeignKey(t => t.SaleSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // SaleTransaction
        modelBuilder.Entity<SaleTransaction>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<SaleTransaction>()
            .HasMany(t => t.Items)
            .WithOne(i => i.Transaction)
            .HasForeignKey(i => i.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        // SaleItem
        modelBuilder.Entity<SaleItem>()
            .HasKey(i => i.Id);
    }
}
