using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Data;

[Index("Name", Name = "UQ__Categori__737584F655AD9643", IsUnique = true)]
public partial class Category
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
    