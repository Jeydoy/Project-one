using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CafeDomain.Model;

public partial class OrderItem: Entity
{
    //public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    [Required]
    [Range(0.1, 100000, ErrorMessage = "Ціна має бути в межах 0.1 та 100000")]
    public decimal PriceForNow { get; set; }

    [Required]
    [Range(1,100, ErrorMessage = "Кількість має бути в межах 1 та 100")]
    public int? Amount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
