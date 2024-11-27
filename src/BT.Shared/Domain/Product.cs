using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BT.Shared.Domain.DTO.Product;

namespace BT.Shared.Domain
{
    public class Product
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "The title field has a max length of 200 characters.")]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = "The description field has a max length of 1000 characters.")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public string? CategoryId { get; set; }

        public Category? Category { get; set; }

        public string GetFormattedPrice() => Price!.Value.ToString("0.00");

        public ICollection<ProductImage>? Images { get; set; }
    }
}
