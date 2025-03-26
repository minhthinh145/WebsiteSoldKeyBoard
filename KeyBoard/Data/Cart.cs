using System;
using System.Collections.Generic;

namespace KeyBoard.Data;

public partial class Cart
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ApplicationUser User { get; set; } = null!;
}
