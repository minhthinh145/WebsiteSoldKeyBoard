namespace KeyBoard.DTOs.HoaDonsDTOs
{
    public class ChiTietHoaDonDTO
    {
        public int MaCt { get; set; }
        public int MaHd { get; set; }
        public Guid MaHh { get; set; }
        public string TenHh { get; set; } = null!;
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public decimal GiamGia { get; set; }
    }
}
