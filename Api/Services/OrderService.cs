using Api.Data;
using Api.DTOs;
using Api.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class OrderService(IApplicationDbContext dbContext, IPublishEndpoint publishEndpoint) : IOrderService
    {
        private readonly IApplicationDbContext _dbContext = dbContext;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public async Task<CreateOrderResponse> PlaceOrderAsync(CreateOrderRequest request)
        {
            var response = new CreateOrderResponse();

            var productIds = new HashSet<int>(request.Items.Select(i => i.Id));

            var availableProducts = await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            // Check for missing products
            if (availableProducts.Count != productIds.Count)
            {
                var missing = productIds.Except(availableProducts.Select(p => p.Id));

                response.Message = $"The following products are not available: {string.Join(", ", missing)}";
                response.Status = "Declined";
                return response;
            }

            // Check for quantity availability
            var insufficientStock = new HashSet<string>();
            foreach (var item in request.Items)
            {
                var product = availableProducts.First(p => p.Id == item.Id);
                if (product.Quantity < item.Quantity)
                {
                    insufficientStock.Add($"{product.Name} (requested: {item.Quantity}, available: {product.Quantity})");
                }
            }

            if (insufficientStock.Count != 0)
            {
                response.Message = $"Insufficient stock for: {string.Join("; ", insufficientStock)}";
                response.Status = "Declined";
                return response;
            } 

            // Create order
            var orderEvent = new OrderCreatedEvent
            {
                CustomerId = Random.Shared.Next(1, 10000),
                ShippingAddress = $"{request.ShippingAddress.Street}, {request.ShippingAddress.City}, {request.ShippingAddress.PostalCode}", 
                Items = [.. request.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    Quantity = i.Quantity
                })]
            }; 

            await _publishEndpoint.Publish(orderEvent);

            return new CreateOrderResponse
            {
                Status = "Pending",
                Message = "Order placed"
            };
        }

    }
}
