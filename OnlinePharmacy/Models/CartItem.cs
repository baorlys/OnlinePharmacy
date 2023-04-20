using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace OnlinePharmacy.Models
{
    public class CartItem
    {

        public int quantity { set; get; }

        public Product? product { set; get; }
    }
}
