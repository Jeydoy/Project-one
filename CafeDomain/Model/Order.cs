using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CafeDomain.Model;

public partial class Order:Entity
{
   // public int Id { get; set; }

    public int StatusId { get; set; }

    public int UserId { get; set; }

    [Range(0.1, 100000, ErrorMessage = "Ціна має бути в межах 0.1 та 100000")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Адреса є обов'язковою")]
    [StringLength(50)]
    public string? Adress { get; set; }

    [Required(ErrorMessage = "Спосіб оплати є обов'язковим")]
    [StringLength(30)]
    public string Payment { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrdersHistory> OrdersHistories { get; set; } = new List<OrdersHistory>();

    public virtual OrdersStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
