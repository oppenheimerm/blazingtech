
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.Admin
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
