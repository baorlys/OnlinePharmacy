using System;
using System.Collections.Generic;

namespace OnlinePharmacy.Models;

public partial class Menu
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string Type { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Url { get; set; }

    public string? Meta { get; set; }

    public bool Hide { get; set; }

    public int Order { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
