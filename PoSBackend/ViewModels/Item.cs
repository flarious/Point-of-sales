using System.ComponentModel.DataAnnotations;

namespace PoSBackend.ViewModels
{
    public class Item
    {
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public int? Unit_id { get; set; }
    }

    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int Unit_id { get; set; }
    }
}
