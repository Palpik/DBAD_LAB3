using System;
using System.Collections.Generic;

namespace AdAgency.Domain;

public partial class Departament
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}
