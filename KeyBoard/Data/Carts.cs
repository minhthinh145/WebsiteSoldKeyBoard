using System;
using System.Collections.Generic;

namespace KeyBoard.Data;

public partial class Carts
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
