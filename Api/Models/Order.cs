namespace Api.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int CustomerId { get; set; } 
        public string ShippingAddress { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<OrderItem> OrderItems { get; set; } = new();
    }
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; } 
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }

    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    } 
}
