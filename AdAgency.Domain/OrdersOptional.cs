using System;
using System.Collections.Generic;

namespace AdAgency.Domain;

public partial class OrdersOptional
{
    public int Id { get; set; }

    public int OptionId { get; set; }

    public int OrderId { get; set; }

    public virtual OptionalService Option { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
