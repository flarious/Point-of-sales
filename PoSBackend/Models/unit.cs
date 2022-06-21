using System;
using System.Collections.Generic;

namespace PoSBackend.Models
{
    public partial class unit
    {
        public unit()
        {
            items = new HashSet<item>();
            orders = new HashSet<order>();
        }

        public int id { get; set; }
        public string name { get; set; } = null!;

        public virtual ICollection<item> items { get; set; }
        public virtual ICollection<order> orders { get; set; }
    }
}
