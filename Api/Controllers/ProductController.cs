using Api.DTOs;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService service) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await service.GetProductsAsync();
            return result != null ? Success(result, "Product loaded.") : Failure<IEnumerable<ProductDto>>("Failed to load products.");
        }
    }
}
