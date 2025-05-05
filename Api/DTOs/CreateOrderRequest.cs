namespace Api.DTOs
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; } 
        public List<OrderItemDto> Items { get; set; } = new();
        public ShippingAddressDto ShippingAddress { get; set; } = new();
    }

}
