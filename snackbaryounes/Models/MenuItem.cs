using System;
using System.Collections.Generic;

namespace snackbaryounes.Models;

public partial class MenuItem
{
    public int MenuItemId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool Status { get; set; }

    public int? ProductCategoryId { get; set; }

    public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ProductCategory? ProductCategory { get; set; }
}
