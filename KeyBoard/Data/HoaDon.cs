using System;
using System.Collections.Generic;

namespace KeyBoard.Data;

public partial class HoaDon
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

    public int MaTrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual TrangThai MaTrangThaiNavigation { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
