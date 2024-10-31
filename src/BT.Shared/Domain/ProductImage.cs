
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BT.Shared.Domain
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? ImageName { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public string? ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
