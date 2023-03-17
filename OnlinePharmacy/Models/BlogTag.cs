using System;
using System.Collections.Generic;

namespace OnlinePharmacy.Models;

public partial class BlogTag
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Frequency { get; set; }
}
