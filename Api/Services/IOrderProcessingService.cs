using Api.Events;

namespace Api.Services
{
    public interface IOrderProcessingService
    {
        Task ProcessOrderAsync(OrderCreatedEvent order);
    }
}
