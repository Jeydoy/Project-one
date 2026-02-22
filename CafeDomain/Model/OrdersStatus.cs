using System;
using System.Collections.Generic;

namespace CafeDomain.Model;

public partial class OrdersStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
