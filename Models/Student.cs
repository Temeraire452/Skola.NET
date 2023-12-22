using System;
using System.Collections.Generic;

namespace Skola.NET.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? Namn { get; set; }

    public string? Personnummer { get; set; }

    public int? FkklassId { get; set; }

    public virtual ICollection<Betyg> Betygs { get; set; } = new List<Betyg>();

    public virtual Klass? Fkklass { get; set; }
}
