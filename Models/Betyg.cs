using System;
using System.Collections.Generic;

namespace Skola.NET.Models;

public partial class Betyg
{
    public int BetygId { get; set; }

    public string? Betyg1 { get; set; }

    public DateTime? Datum { get; set; }

    public int? FkstudentId { get; set; }

    public int? Fkkurs { get; set; }

    public int? FkpersonalId { get; set; }

    public virtual Kur? FkkursNavigation { get; set; }

    public virtual Personal? Fkpersonal { get; set; }

    public virtual Student? Fkstudent { get; set; }
}
