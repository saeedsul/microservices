using Api.Data;
using Api.DTOs;
using Api.Events;
using Api.Models;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Test.Services
{
    public class OrderProcessingServiceTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;

        public OrderProcessingServiceTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
        }

        [Fact]
        public async Task ProcessOrderAsync_ProcessesOrderCorrectly()
        {
            // Arrange 
            var orderCreatedEvent = new OrderCreatedEvent
            {
                CustomerId = 1,
                ShippingAddress = "123 Main St, Metropolis, 00000",
                Items =
                [
                    new OrderItemDto { Id = 1, Quantity = 2 },
                    new OrderItemDto { Id = 2, Quantity = 1 }
                ]
            };

            // Arrange 
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product A", Price = 100, Quantity = 10 },
                new() { Id = 2, Name = "Product B", Price = 200, Quantity = 5 }
            }.AsQueryable();
             
            var mockProductDbSet = products.BuildMockDbSet();
             
            _mockDbContext.Setup(db => db.Products).Returns(mockProductDbSet.Object);

            var mockOrderDbSet = new Mock<DbSet<Order>>();
            _mockDbContext.Setup(db => db.Orders).Returns(mockOrderDbSet.Object);


            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);  
             
            var orderProcessingService = new OrderProcessingService(_mockDbContext.Object);

            // Act 
            await orderProcessingService.ProcessOrderAsync(orderCreatedEvent);

            // Assert 
            var updatedProduct = products.First(p => p.Id == 1);
            var updatedProduct2 = products.First(p => p.Id == 2);

            Assert.Equal(8, updatedProduct.Quantity);  
            Assert.Equal(4, updatedProduct2.Quantity);  
             
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
             
            _mockDbContext.Verify(db => db.Orders.Add(It.IsAny<Order>()), Times.Once);
        }
    }
}
