using System.ComponentModel.DataAnnotations;

namespace PoSBackend.ViewModels
{
    public class Unit
    {
        [Required]
        public string Name { get; set; }
    }

    public class UnitViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
