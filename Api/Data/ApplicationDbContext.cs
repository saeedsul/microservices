using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage; 

namespace Api.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator dbCreater)
            {
                // Create Database 
                if (!dbCreater.CanConnect())
                {
                    dbCreater.Create();
                }

                // Create Tables
                if (!dbCreater.HasTables())
                {
                    dbCreater.CreateTables();
                }
            }
        }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> Items { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.CustomerId).IsRequired();
                entity.Property(o => o.ShippingAddress).IsRequired();
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");

                entity.HasMany(o => o.OrderItems)
                      .WithOne(i => i.Order)
                      .HasForeignKey(i => i.OrderId);
            });

            // OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.ProductId).IsRequired();
            });

            // Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

            // 🔰 Seed Data
            var orderId = Guid.NewGuid();

            // Seed Product
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id =1,
                    Name = "Laptop",
                    Price = 1250.00m,
                    Quantity = 50
                },
                new Product
                {
                    Id = 2,
                    Name = "Mouse",
                    Price = 74.99m,
                    Quantity = 50
                },
                new Product
                {
                    Id = 3,
                    Name = "Keyboard",
                    Price = 22.99m,
                    Quantity = 50
                }
            );

            // Seed Order
            modelBuilder.Entity<Order>().HasData(new Order
            {
                Id = orderId,
                CustomerId = 1,
                ShippingAddress = "221B Baker Street, London, NW1 6XE",
                TotalAmount = 150.00m,
                CreatedAt = DateTime.UtcNow
            });

            // Seed OrderItems
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = 1,
                    OrderId = orderId,
                    ProductId =1,
                    Quantity = 2
                },
                new OrderItem
                {
                    Id = 2,
                    OrderId = orderId,
                    ProductId = 2,
                    Quantity = 1
                }
            );
        }

    }

}
