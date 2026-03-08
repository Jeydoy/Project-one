using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CafeDomain.Model;

public partial class Product: Entity
{
   // public int Id { get; set; }

    public string Photo { get; set; } = null!;

    public bool? IsAvaliable { get; set; }

    [Required]
    [Range(0.1,100000, ErrorMessage = "Ціна має бути в межах 0.1 та 100000")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(150)]
    public string Description { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
