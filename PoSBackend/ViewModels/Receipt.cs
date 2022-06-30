using System.ComponentModel.DataAnnotations;

namespace PoSBackend.ViewModels
{
    public class Receipt
    {
        [Required]
        public string Date { get; set; } = string.Empty;
        [Required]
        public decimal? Total_amount { get; set; }
        [Required]
        public decimal? Total_discount_amount { get; set; }
        [Required]
        public decimal? Subtotal { get; set; }
        [Required]
        public decimal? Trade_discount { get; set; }
        [Required]
        public decimal? Grand_total { get; set; }
        [Required]
        public List<Order> Orders { get; set; } = new List<Order>();
    }

    public class ReceiptViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public decimal Grand_total { get; set; }
    }

    public class ReceiptDetailViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public decimal Total_amount { get; set; }
        public decimal Total_discount_amount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Trade_discount { get; set; }
        public decimal Grand_total { get; set; }
        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
    }
}
