using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> Items { get; set; }
        DbSet<Product> Products { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
    }

}
