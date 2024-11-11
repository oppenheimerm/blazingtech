using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.Admin
{
    public class AddUserToRoleDTO
    {
        [Required]
        public Guid? BTUserId { get; set; }
        [Required]
        public string? RoleCode { get; set; }
    }
}
