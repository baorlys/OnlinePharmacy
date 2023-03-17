using System;
using System.Collections.Generic;

namespace OnlinePharmacy.Models;

public partial class ProductInventory
{
    public int Id { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Product IdNavigation { get; set; } = null!;
}
