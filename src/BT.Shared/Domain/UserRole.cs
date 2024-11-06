
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain
{
    /// <summary>
    /// <see cref="UserRole"/> for the join entity in addition to the existing types for <see cref="BTUser"/> and <see cref="Role"/>:
    /// </summary>
    public class UserRole
    {
        [Required]
        [ForeignKey(nameof(Category))]
        public Guid? BTUserId { get; set; }

        public BTUser? BTUser { get; set; }

        [Required]
        [ForeignKey(nameof(Role))]
        public string? RoleCode { get; set; }

        public Role? Role { get; set; }

        [Required]
        public DateTime? TimeStamp { get; set; }
    }
}
