using Api.Data;
using Api.DTOs;
using Api.Events;
using Api.Models;
using Api.Services;
using MassTransit;
using MockQueryable.Moq;
using Moq;

namespace Test.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;

        public OrderServiceTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        }

        [Fact]
        public async Task PlaceOrderAsync_ReturnsDeclined_WhenProductMissing()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                Items = [
                    new() { Id = 1, Quantity = 2 },
                    new() { Id = 2, Quantity = 1 }
                ],
                ShippingAddress = new ShippingAddressDto
                {
                    Street = "123 Street",
                    City = "Test City",
                    PostalCode = "12345"
                }
            };

            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product A", Quantity = 10, Price = 100 }
            }.AsQueryable();

            _mockDbContext.Setup(x => x.Products).Returns(products.BuildMockDbSet().Object);

            var service = new OrderService(_mockDbContext.Object, _mockPublishEndpoint.Object);

            // Act
            var result = await service.PlaceOrderAsync(request);

            // Assert
            Assert.Equal("Declined", result.Status);
            Assert.Contains("not available", result.Message);
            _mockPublishEndpoint.Verify(p => p.Publish(It.IsAny<OrderCreatedEvent>(), default), Times.Never);
        }

        [Fact]
        public async Task PlaceOrderAsync_ReturnsDeclined_WhenInsufficientStock()
        {
            // Arrange
            var request = CreateSampleRequest();

            var existingProducts = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", Quantity = 2 },
                new() { Id = 2, Name = "Product 2", Quantity = 0 }
            }.AsQueryable();

            _mockDbContext.Setup(x => x.Products).Returns(existingProducts.BuildMockDbSet().Object);

            var service = new OrderService(_mockDbContext.Object, _mockPublishEndpoint.Object);

            // Act
            var result = await service.PlaceOrderAsync(request);

            // Assert
            Assert.Equal("Declined", result.Status);
            Assert.Contains("Insufficient stock", result.Message);
            _mockPublishEndpoint.Verify(p => p.Publish(It.IsAny<OrderCreatedEvent>(), default), Times.Never);
        }

        [Fact]
        public async Task PlaceOrderAsync_ReturnsPending_WhenValid()
        {
            // Arrange
            var request = CreateSampleRequest();

            var existingProducts = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", Quantity = 10 },
                new() { Id = 2, Name = "Product 2", Quantity = 10 }
            }.AsQueryable();

            _mockDbContext.Setup(x => x.Products).Returns(existingProducts.BuildMockDbSet().Object);
            _mockPublishEndpoint.Setup(p => p.Publish(It.IsAny<OrderCreatedEvent>(), default))
                .Returns(Task.CompletedTask);

            var service = new OrderService(_mockDbContext.Object, _mockPublishEndpoint.Object);

            // Act
            var result = await service.PlaceOrderAsync(request);

            // Assert
            Assert.Equal("Pending", result.Status);
            Assert.Equal("Order placed", result.Message);
            _mockPublishEndpoint.Verify(p => p.Publish(It.IsAny<OrderCreatedEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task PlaceOrderAsync_PublishesEvent_WithCorrectData()
        {
            // Arrange
            var request = CreateSampleRequest();
            var existingProducts = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", Quantity = 10 },
                new() { Id = 2, Name = "Product 2", Quantity = 10 }
            };

            _mockDbContext.Setup(x => x.Products).Returns(existingProducts.BuildMockDbSet().Object);

            var service = new OrderService(_mockDbContext.Object, _mockPublishEndpoint.Object);

            // Act
            var result = await service.PlaceOrderAsync(request);

            // Assert
            _mockPublishEndpoint.Verify(p => p.Publish(It.IsAny<OrderCreatedEvent>(), default), Times.Once); 

            Assert.Equal("Pending", result.Status);
            Assert.Equal("Order placed", result.Message);
        }

        // Helpers
        private static CreateOrderRequest CreateSampleRequest()
        {
            return new CreateOrderRequest
            {
                ShippingAddress = new ShippingAddressDto
                {
                    Street = "123 Main St",
                    City = "City",
                    PostalCode = "PS1 C0D"
                },
                Items = [
                    new() { Id = 1, Quantity = 2 },
                    new() { Id = 2, Quantity = 1 }
                ]
            };
        }
    }
}
