
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BT.Shared.Domain
{
    public class ProductImage
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Image file name cannot exceed 256 characters.")]
        public string? ImageName { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public int? ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
