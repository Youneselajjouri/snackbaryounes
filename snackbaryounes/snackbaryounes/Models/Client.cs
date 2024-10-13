using System;
using System.Collections.Generic;

namespace snackbaryounes.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
