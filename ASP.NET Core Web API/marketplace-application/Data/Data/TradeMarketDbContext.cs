using System;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data;

public class TradeMarketDbContext : DbContext
{
    public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; } = null!;

    public DbSet<Customer> Customers { get; set; } = null!;

    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Receipt> Receipts { get; set; } = null!;

    public DbSet<ReceiptDetail> ReceiptsDetails { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        // Customer - Person relation
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Person)
            .WithMany()
            .HasForeignKey(c => c.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product - ProductCategory relation
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.ProductCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Receipt - Customer relation
        modelBuilder.Entity<Receipt>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Receipts)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // ReceiptDetail - Receipt relation
        modelBuilder.Entity<ReceiptDetail>()
            .HasOne(rd => rd.Receipt)
            .WithMany(r => r.ReceiptDetails)
            .HasForeignKey(rd => rd.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        // ReceiptDetail - Product relation
        modelBuilder.Entity<ReceiptDetail>()
            .HasOne(rd => rd.Product)
            .WithMany(p => p.ReceiptDetails)
            .HasForeignKey(rd => rd.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Decimal precision configurations
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<ReceiptDetail>()
            .Property(rd => rd.UnitPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<ReceiptDetail>()
            .Property(rd => rd.DiscountUnitPrice)
            .HasColumnType("decimal(18,2)");
    }
}