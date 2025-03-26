namespace KeyBoard.DTOs.HoaDonsDTOs
{
    public class HoaDonDTO
    {
        public int MaHd { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime NgayDat { get; set; }
        public DateTime? NgayCan { get; set; }
        public DateTime? NgayGiao { get; set; }
        public string? HoTen { get; set; }
        public string DiaChi { get; set; } = null!;
        public string? SoDienThoai { get; set; }
        public string CachThanhToan { get; set; } = null!;
        public string CachVanChuyen { get; set; } = null!;
        public decimal PhiVanChuyen { get; set; }
        public string TrangThai { get; set; } = null!; // Chuyển MaTrangThai thành chuỗi trạng thái
        public string? GhiChu { get; set; }
        public List<ChiTietHoaDonDTO> ChiTietHoaDons { get; set; } = new();
    }
}
