namespace KeyBoard.DTOs
{
    public class CartItemDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; }

    }
}
