
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.Product
{

    public record ProductImageDTO(
        int? Id,
        [Required, MaxLength(256, ErrorMessage = "Image name, max lenght is 256 characters.")] string? ImageName,
        [Required] int? ProductId
        );
}
