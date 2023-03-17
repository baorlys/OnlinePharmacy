using System;
using System.Collections.Generic;

namespace OnlinePharmacy.Models;

public partial class Product
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Desc { get; set; }

    public string? Image { get; set; }

    public string? Price { get; set; }

    public string? Unit { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ProductCategory? Category { get; set; }

    public virtual ProductInventory? ProductInventory { get; set; }
}
