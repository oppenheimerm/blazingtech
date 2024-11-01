
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO
{
    public record ProductDTO
    (
        int? Id,
        [Required, MaxLength(200, ErrorMessage = "Product title max lenght is 200 characters.")] string? Title,
        [Required, MaxLength(1000, ErrorMessage = "Product description max lenght is 1000 characters.")] string? Description,
        [Required] decimal? Price,
        [Required] string? CategoryId,
        ICollection<ProductAttribute>? Attributes,
        ICollection<ProductImage>? Images
    );
}
