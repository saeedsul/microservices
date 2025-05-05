using Api.Data;
using Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class ProductService(IApplicationDbContext dbContext) : IProductService
    {
        private readonly IApplicationDbContext _dbContext = dbContext;

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _dbContext.Products
           .Select(p => new ProductDto
           {
               Id = p.Id,
               Name = p.Name, 
               Price = p.Price 
           })
           .ToListAsync();

            return products;
        }
    }
}
