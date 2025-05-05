using Moq; 
using Microsoft.AspNetCore.Mvc;
using Api.Controllers;
using Api.Services; 
using Api.DTOs;
using Api.Helpers;
using Shouldly;

namespace Test.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrderController _sut;

        public OrderControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _sut = new OrderController(_mockOrderService.Object);
        }

        [Fact]
        public async Task PlaceOrder_ReturnsOkResult_WhenOrderIsPlacedSuccessfully()
        {
            // Arrange
            var createOrderRequest = new CreateOrderRequest
            {
                CustomerId = 1,
                ShippingAddress = new ShippingAddressDto
                {
                    Street = "123 Main St",
                    City = "City",
                    PostalCode = "PS1 C0D"
                },
                Items =
                [
                    new OrderItemDto { Id = 1, Quantity = 2 },
                    new OrderItemDto { Id = 2, Quantity = 1 }
                ]
            };
          
            var expectedResponse = new CreateOrderResponse { OrderId = "123", Status = "Success" };
            _mockOrderService.Setup(service => service.PlaceOrderAsync(createOrderRequest))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.PlaceOrder(createOrderRequest);

            // Assert 
            var okResult = result as OkObjectResult;
            okResult.ShouldNotBeNull();

            var response = okResult.Value as ApiResponse<CreateOrderResponse>;
            response.ShouldNotBeNull();
            response!.Success.ShouldBeTrue();
            response.Data.ShouldNotBeNull(); 

            _mockOrderService.Verify(service => service.PlaceOrderAsync(createOrderRequest), Times.Once);
        }

        [Fact]
        public async Task PlaceOrder_ReturnsFailure_WhenOrderIsNotCreated()
        {
            // Arrange 
            var createOrderRequest = new CreateOrderRequest
            {
                CustomerId = 1,
                ShippingAddress = new ShippingAddressDto
                {
                    Street = "123 Main St",
                    City = "City",
                    PostalCode = "PS1 C0D"
                },
                Items =
                [
                    new OrderItemDto { Id = 1, Quantity = 2 },
                    new OrderItemDto { Id = 2, Quantity = 1 }
                ]
            }; 

            _mockOrderService.Setup(service => service.PlaceOrderAsync(createOrderRequest))
                .ReturnsAsync((CreateOrderResponse)null);

            // Act
            var result = await _sut.PlaceOrder(null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.ShouldNotBeNull();

            var response = badRequestResult.Value as ApiResponse<CreateOrderResponse>;
            response.ShouldNotBeNull();
            response!.Success.ShouldBeFalse();
            response.Message.ShouldBe("Failed to place order.");
             
            _mockOrderService.Verify(service => service.PlaceOrderAsync(createOrderRequest), Times.Never);
        }
    }
}

