using Api.Data;
using Api.Models;
using Api.Services;
using MockQueryable.Moq;
using Moq;

namespace Test.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;

        public ProductServiceTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
        }

        [Fact]
        public async Task GetProductsAsync_ReturnsProductsDtoList()
        {
            // Arrange 
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product A", Price = 100 },
                new() { Id = 2, Name = "Product B", Price = 200 }
            }.AsQueryable();
             
            var mockProductDbSet = products.BuildMockDbSet();   
             
            _mockDbContext.Setup(db => db.Products).Returns(mockProductDbSet.Object);
             
            var productService = new ProductService(_mockDbContext.Object);

            // Act 
            var result = await productService.GetProductsAsync();
             
            var productList = result.ToList();   

            // Assert

            Assert.NotNull(productList);
            Assert.Equal(2, productList.Count);
            Assert.Equal("Product A", productList[0].Name);
            Assert.Equal(100, productList[0].Price);
            Assert.Equal("Product B", productList[1].Name);
            Assert.Equal(200, productList[1].Price);
        }
    }
}
