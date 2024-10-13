using System;
using System.Collections.Generic;

namespace snackbaryounes.Models;

public partial class ProductCategory
{
    public int ProductCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
