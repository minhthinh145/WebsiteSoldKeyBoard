using System;
using System.Collections.Generic;

namespace KeyBoard.Data;

public partial class TrangThai
{
    public int MaTrangThai { get; set; }

    public string TenTrangThai { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<Bill> HoaDons { get; set; } = new List<Bill>();
}
