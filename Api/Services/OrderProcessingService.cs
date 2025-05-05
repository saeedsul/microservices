using Api.Data;
using Api.Events;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class OrderProcessingService(IApplicationDbContext dbContext) : IOrderProcessingService
    {
        private readonly IApplicationDbContext _dbContext = dbContext;

        public async Task ProcessOrderAsync(OrderCreatedEvent order)
        {
            var productIds = order.Items.Select(i => i.Id).ToHashSet();

            var products = await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            // Deduct stock and calculate total
            decimal totalAmount = 0;
            foreach (var item in order.Items)
            {
                var product = products.First(p => p.Id == item.Id);
                product.Quantity -= item.Quantity;
                totalAmount += product.Price * item.Quantity;
            }

            var newOrder = new Order
            {
                CustomerId = order.CustomerId,
                ShippingAddress = order.ShippingAddress,
                TotalAmount = totalAmount,
                OrderItems = order.Items.Select(i => new OrderItem
                {
                    ProductId = i.Id,
                    Quantity = i.Quantity
                }).ToList(),
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Orders.Add(newOrder);

            await _dbContext.SaveChangesAsync();
        }
    }
}
