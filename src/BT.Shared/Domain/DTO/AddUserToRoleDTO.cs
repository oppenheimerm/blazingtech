
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BT.Shared.Domain.DTO
{
    public class AddUserToRoleDTO
    {
        [Required]
        public Guid? BTUserId { get; set; }
        [Required]
        public string? RoleCode { get; set; }
    }
}
