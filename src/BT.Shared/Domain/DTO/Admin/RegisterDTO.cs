
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.Admin
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(2, ErrorMessage = "Firstname requires a minimum lenght of 2 characters")]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Last requires a minimum lenght of 2 characters")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";

        [StringLength(11, ErrorMessage = "The max lenght for phone number is 10 characters")]
        public string? Mobile { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Address has a maximum of 256 characters.")]
        public string? Address { get; set; }
    }
}
