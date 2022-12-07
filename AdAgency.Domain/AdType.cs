using System;
using System.Collections.Generic;

namespace AdAgency.Domain;

public partial class AdType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<AdPlace> AdPlaces { get; } = new List<AdPlace>();
}
