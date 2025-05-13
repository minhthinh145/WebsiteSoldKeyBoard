namespace KeyBoard.DTOs
{
    public class CartDTO
    {
        public List<CartItemDTO> Items { get; set; } = new(); 
        public Decimal TotalPrice {  get; set; }
    }
}
