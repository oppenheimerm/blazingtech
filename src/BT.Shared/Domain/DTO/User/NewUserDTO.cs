
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BT.Shared.Domain.DTO.Admin;

namespace BT.Shared.Domain.DTO.User
{
    public class NewUserDTO
    {
        public NewUserDTO()
        {
            this.Id = Guid.NewGuid();
            this.JoinDate = DateTime.UtcNow;
        }

        [Required]
        public Guid Id { get; }

        public DateTime? JoinDate { get; } = DateTime.Now;

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

        [MaxLength(100, ErrorMessage = "Filename has a maximum of 100 characters.")]
        public string? ProfilePicture { get; set; }

        [MaxLength(12, ErrorMessage = "Mobile number has a maximum of 12 characters.")]
        public string? Mobile { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";
        public List<RoleLiteDTO>? UserRoles { get; set; }
    }
}
