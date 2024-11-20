
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.Product
{
    public record ProductAttributeDTO
    (
        int? Id,
        [Required] string? Key,
        [Required] string? Value,
        [Required] int? ProductId
    );
}
