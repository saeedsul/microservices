namespace Api.DTOs
{
    public class CreateOrderResponse
    {
        public string OrderId { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public decimal TotalAmount { get; set; }
        public string? Message { get; set; }
    }

}
