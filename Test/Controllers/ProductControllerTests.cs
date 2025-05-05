using Api.Controllers;
using Api.DTOs;
using Api.Helpers;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;

namespace Test.Controllers
{

    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductController _sut;

        public ProductControllerTests()
        { 
            _mockProductService = new Mock<IProductService>(); 
            _sut = new ProductController(_mockProductService.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WhenProductsAreLoadedSuccessfully()
        {
            // Arrange 
            var products = new List<ProductDto>
            {
                new() { Id = 1, Name = "Product A", Price = 100 },
                new() { Id = 2, Name = "Product B", Price = 200 }
            };
             
            _mockProductService.Setup(service => service.GetProductsAsync())
                .ReturnsAsync(products);

            // Act 
            var result = await _sut.GetProducts();

            // Assert 
            var okResult = result as OkObjectResult;
            okResult.ShouldNotBeNull();

            var response = okResult.Value as ApiResponse<IEnumerable<ProductDto>>;
            response.ShouldNotBeNull();
            response!.Success.ShouldBeTrue();
            response.Data.ShouldBe(products);  
        }

        [Fact]
        public async Task GetProducts_ReturnsFailure_WhenNoProductsAreLoaded()
        {
            // Arrange 
            _mockProductService.Setup(service => service.GetProductsAsync())
                .ReturnsAsync((IEnumerable<ProductDto>)null);

            // Act 
            var result = await _sut.GetProducts();

            // Assert 
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.ShouldNotBeNull();

            var response = notFoundResult.Value as ApiResponse<IEnumerable<ProductDto>>;
            response.ShouldNotBeNull();
            response!.Success.ShouldBeFalse();
            response.Message.ShouldBe("Failed to load products."); 
        }
    }
}

