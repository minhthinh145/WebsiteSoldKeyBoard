using System;
using System.Collections.Generic;

namespace KeyBoard.Data;

public partial class Order
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string OrderStatus { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual AspNetUser User { get; set; } = null!;
}
