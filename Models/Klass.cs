using System;
using System.Collections.Generic;

namespace Skola.NET.Models;

public partial class Klass
{
    public int KlassId { get; set; }

    public string? Klassnamn { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
