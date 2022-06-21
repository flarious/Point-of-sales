using System;
using System.Collections.Generic;

namespace PoSBackend.Models
{
    public partial class item
    {
        public int id { get; set; }
        public string code { get; set; } = null!;
        public string name { get; set; } = null!;
        public decimal price { get; set; }
        public int unit { get; set; }

        public virtual unit unitNavigation { get; set; } = null!;
    }
}
