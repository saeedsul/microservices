using Api.DTOs;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IOrderService service) : BaseController
    {
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderRequest request)
        {
            var result = await service.PlaceOrderAsync(request);
            return result != null ? Success(result, "Order placed.") : BadRequest<CreateOrderResponse>("Failed to place order."); 
        }
    }
}
