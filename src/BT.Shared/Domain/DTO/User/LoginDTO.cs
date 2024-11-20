
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.User
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
