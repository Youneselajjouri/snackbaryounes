using System;
using System.Collections.Generic;

namespace snackbaryounes.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? ClientId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
