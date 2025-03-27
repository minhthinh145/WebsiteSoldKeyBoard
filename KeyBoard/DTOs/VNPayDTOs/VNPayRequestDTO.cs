namespace KeyBoard.DTOs.VNPayDTOs
{
    public class VNPayRequestDTO
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int MaHd { get; set; }  // Mã hóa đơn
    }
}
