    namespace PoSBackend.ViewModels
{
    public class Receipt
    {
        public string Date { get; set; } = string.Empty;
        public decimal Total_amount { get; set; }
        public decimal Total_discount_amount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Trade_discount { get; set; }
        public decimal Grand_total { get; set; }
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
