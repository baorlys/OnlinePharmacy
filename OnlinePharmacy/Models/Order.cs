using System;
using System.Collections.Generic;

namespace OnlinePharmacy.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public decimal TotalPrice { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public bool PaymentStatus { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public decimal ShippingFee { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
