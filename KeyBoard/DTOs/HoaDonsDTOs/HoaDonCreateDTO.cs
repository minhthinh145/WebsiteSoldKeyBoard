namespace KeyBoard.DTOs.HoaDonsDTOs
{
    public class HoaDonCreateDTO
    {
        public string UserId { get; set; } = null!;
        public DateTime? NgayCan { get; set; } // Ngày cần giao (optional)
        public string? HoTen { get; set; }
        public string DiaChi { get; set; } = null!;
        public string? SoDienThoai { get; set; }
        public string CachThanhToan { get; set; } = null!;
        public string CachVanChuyen { get; set; } = null!;
        public decimal PhiVanChuyen { get; set; }
        public string? GhiChu { get; set; }
    }
}
