
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BT.Shared.Domain
{
    public class ProductAttribute
    {
        [Key]
        public string? Id { get; set; }

        [Required]
        public string? Key { get; set; }

        [Required]
        public string? Value { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public string? ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
