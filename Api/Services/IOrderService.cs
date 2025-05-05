using Api.DTOs;

namespace Api.Services
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> PlaceOrderAsync(CreateOrderRequest request);
    }
}
