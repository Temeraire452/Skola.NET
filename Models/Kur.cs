using System;
using System.Collections.Generic;

namespace Skola.NET.Models;

public partial class Kur
{
    public int KursId { get; set; }

    public string? Kursnamn { get; set; }

    public virtual ICollection<Betyg> Betygs { get; set; } = new List<Betyg>();
}
