using System;
using System.Collections.Generic;

namespace AdAgency.Domain;

public partial class OptionalService
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Cost { get; set; }

    public virtual ICollection<OrdersOptional> OrdersOptionals { get; } = new List<OrdersOptional>();
}
