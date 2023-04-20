using System;
using System.Collections.Generic;

namespace OnlinePharmacy.Models;

public partial class Blog
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Meta { get; set; }

    public string? Author { get; set; }

    public string? Content { get; set; }

    public string? Thumb { get; set; }

    public string? Summary { get; set; }

    public string? Tags { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
