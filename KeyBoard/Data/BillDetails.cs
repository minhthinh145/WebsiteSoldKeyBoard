using System;
using System.Collections.Generic;

namespace KeyBoard.Data;

public partial class BillDetails
{
    public int MaCt { get; set; }

    public int MaHd { get; set; }

    public Guid MaHh { get; set; }

    public decimal DonGia { get; set; }

    public int SoLuong { get; set; }

    public decimal GiamGia { get; set; }

    public string TenHh { get; set; } = null!;

    public virtual Bill MaHdNavigation { get; set; } = null!;

    public virtual Product MaHhNavigation { get; set; } = null!;
}
