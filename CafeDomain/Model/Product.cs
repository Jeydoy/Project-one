using System;
using System.Collections.Generic;

namespace CafeDomain.Model;

public partial class Product
{
    public int Id { get; set; }

    public string Photo { get; set; } = null!;

    public bool? IsAvaliable { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
