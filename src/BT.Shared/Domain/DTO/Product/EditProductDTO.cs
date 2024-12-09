
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.Product
{
    public class EditProductDTO
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "The title field has a max length of 200 characters.")]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = "The description field has a max length of 1000 characters.")]
        public string? Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        public int? StockQuantity { get; set; }

        [Required]
        public string? CategoryId { get; set; }

        public string GetFormattedPrice() => Price!.Value.ToString("0.00");

        public ICollection<ProductImageDTO>? Images { get; set; }
        public List<ProductSpecficationDTO>? TechSpecs { get; set; }
    }
}
