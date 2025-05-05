using MassTransit;
using Api.Events;
using Api.Services;

namespace Api.Messaging.Consumers
{
    public class OrderCreatedConsumer(IOrderProcessingService service) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var order = context.Message;

            await service.ProcessOrderAsync(order);

            Console.WriteLine($"[Consumer] Received Order: {order.OrderId}");
        }
    }
}
