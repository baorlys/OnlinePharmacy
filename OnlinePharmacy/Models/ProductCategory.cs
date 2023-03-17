using System;
using System.Collections.Generic;

namespace OnlinePharmacy.Models;

public partial class ProductCategory
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string? Name { get; set; }

    public string? Meta { get; set; }

    public string? Desc { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
