namespace KeyBoard.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public List<OrderDetailDTO> OrderDetails { get; set; } = new();
    }
}
