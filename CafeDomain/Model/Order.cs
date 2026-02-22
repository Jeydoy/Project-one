using System;
using System.Collections.Generic;

namespace CafeDomain.Model;

public partial class Order
{
    public int Id { get; set; }

    public int StatusId { get; set; }

    public int UserId { get; set; }

    public decimal Price { get; set; }

    public string? Adress { get; set; }

    public string Payment { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrdersHistory> OrdersHistories { get; set; } = new List<OrdersHistory>();

    public virtual OrdersStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
