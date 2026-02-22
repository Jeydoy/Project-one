using System;
using System.Collections.Generic;

namespace CafeDomain.Model;

public partial class OrdersHistory
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public DateTime? ChangedAt { get; set; }

    public int OldStatusId { get; set; }

    public int NewStatusId { get; set; }

    public virtual Order Order { get; set; } = null!;
}
