using System;
using System.Collections.Generic;

namespace PoSBackend.Models
{
    public partial class order
    {
        public int id { get; set; }
        public int receipt_id { get; set; }
        public int item_id { get; set; }
        public string item_code { get; set; } = null!;
        public string item_name { get; set; } = null!;
        public string item_unit { get; set; } = null!;
        public int unit_id { get; set; }
        public int item_quantity { get; set; }
        public decimal item_price { get; set; }
        public decimal item_discount_percent { get; set; }
        public decimal item_discount_amount { get; set; }
        public decimal item_total_amount { get; set; }

        public virtual item item { get; set; } = null!;
        public virtual receipt receipt { get; set; } = null!;
        public virtual unit unit { get; set; } = null!;
    }
}
