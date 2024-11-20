
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using BT.Shared.Domain.DTO.Admin;

namespace BT.Shared.Domain.DTO.User
{
    public class EditUserDTO
    {
        public Guid Id { get; set; }

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

        //public string? PasswordHash { get; set; }

        [MaxLength(100, ErrorMessage = "Filename has a maximum of 100 characters.")]
        public string? ProfilePicture { get; set; }

        [MaxLength(12, ErrorMessage = "Mobile number has a maximum of 12 characters.")]
        public string? Mobile { get; set; }

        public bool AccountLockedOut { get; set; } = false;

        public List<RoleLiteDTO>? UserRoles { get; set; }
    }
}
