
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BT.Shared.Domain
{
    /// <summary>
    /// Custom <see cref="IdentityUser"/> entity
    /// </summary>
    public class BTUser
    {
        public BTUser()
        {
            this.Id = Guid.NewGuid();
            this.JoinDate = DateTime.UtcNow;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public Guid Id { get; set; }

        public DateTime? JoinDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        [PersonalData]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [PersonalData]
        public string LasttName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [MaxLength(256, ErrorMessage = "Address has a maximum of 256 characters.")]
        public string? Address { get; set; }

        public string? PasswordHash { get; set; }

        [MaxLength(100, ErrorMessage = "Filename has a maximum of 100 characters.")]
        public string? ProfilePicture { get; set; }

        [MaxLength(12, ErrorMessage = "Mobile number has a maximum of 12 characters.")]
        public string? Mobile { get; set; }

        public ICollection<Role>? Roles { get; set; }

        public string? VerificationToken { get; set; }

        public DateTime? Verified { get; set; }

        public bool IsVerified => Verified.HasValue;

        public bool AccountLockedOut { get; set; } = false;

    }
}
