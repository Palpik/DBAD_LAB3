using System;
using System.Collections.Generic;

namespace AdAgency.Domain;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public int? PositionId { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Post? Position { get; set; }
}
