using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Data;

[Table("Cart")]
public partial class Cart
{
    [Key]
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Carts")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Carts")]
    public virtual ApplicationUser User { get; set; } = null!;
}
