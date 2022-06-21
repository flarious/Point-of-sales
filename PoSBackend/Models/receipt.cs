using System;
using System.Collections.Generic;

namespace PoSBackend.Models
{
    public partial class receipt
    {
        public receipt()
        {
            orders = new HashSet<order>();
        }

        public int id { get; set; }
        public string code { get; set; } = null!;
        public DateOnly date { get; set; }
        public decimal total_amount { get; set; }
        public decimal total_discount_amount { get; set; }
        public decimal subtotal { get; set; }
        public decimal trade_discount { get; set; }
        public decimal grand_total { get; set; }

        public virtual ICollection<order> orders { get; set; }
    }
}
