using System.ComponentModel.DataAnnotations;

namespace PoSBackend.ViewModels
{
    public class Order
    {
        [Required]
        public int? Item_id { get; set; }
        [Required]
        public string Item_code { get; set; } = string.Empty;
        [Required]
        public string Item_name { get; set; } = string.Empty;
        [Required]
        public string Item_unit { get; set; } = string.Empty;
        [Required]
        public int? Item_unit_id { get; set; }
        [Required]
        public int? Item_quantity { get; set; }
        [Required]
        public decimal? Item_price { get; set; }
        [Required]
        public decimal? Item_discount_percent { get; set; }
        [Required]
        public decimal? Item_discount_amount { get; set; }
        [Required]
        public decimal? Item_total_amount { get; set; }
    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Item_code { get; set; } = string.Empty;
        public string Item_name { get; set; } = string.Empty;
        public string Item_unit { get; set; } = string.Empty;
        public int Item_quantity { get; set; }
        public decimal Item_price { get; set; }
        public decimal Item_discount_percent { get; set; }
        public decimal Item_discount_amount { get; set; }
        public decimal Item_total_amount { get; set; }
    }
}
