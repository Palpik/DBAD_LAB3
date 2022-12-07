using System;
using System.Collections.Generic;

namespace AdAgency.Domain;

public partial class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? CustomerId { get; set; }

    public int? PlaceId { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrdersOptional> OrdersOptionals { get; } = new List<OrdersOptional>();

    public virtual AdPlace? Place { get; set; }
}
