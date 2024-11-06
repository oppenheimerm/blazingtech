
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BT.Shared.Domain
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(4, ErrorMessage = "Role code must be 4 characters long"), MinLength(4)]
        public string? RoleCode { get; set; }

        [Required]
        public string? RoleName { get; set; }

        [StringLength(50)]
        public string? Description { get; set; }

        public ICollection<BTUser>? Users { get; set; }

    }
}
