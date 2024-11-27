
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.Product
{
    public class AddProductAttributeDTO
    {
        [Required]
        [MaxLength(100, ErrorMessage = "The key field has a max length of 100 characters.")]
        public string? Key { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "The value field has a max length of 200 characters.")]
        public string? Value { get; set; }
    }
}
