using Api.DTOs;

namespace Api.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
    }

}
