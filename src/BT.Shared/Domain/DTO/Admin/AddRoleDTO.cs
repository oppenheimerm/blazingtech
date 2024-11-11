
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.Admin
{
    public class AddRoleDTO
    {
        [Required]
        [MaxLength(4, ErrorMessage = "Role code must be 4 characters long"), MinLength(4)]
        public string? RoleCode { get; set; }

        [Required]
        public string? RoleName { get; set; }

        [StringLength(50)]
        public string? Description { get; set; }
    }
}
