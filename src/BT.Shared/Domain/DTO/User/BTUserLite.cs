
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.User
{
    public class BTUserLite
    {
        public Guid Id { get; set; }

        public DateTime? JoinDate { get; set; } = DateTime.Now;
        public string FirstName { get; set; } = string.Empty;
        public string LasttName { get; set; } = string.Empty;
        public string Email { get; set; } = "";
        public string? Address { get; set; }      
        public string? ProfilePicture { get; set; }
        public string? Mobile { get; set; }  
        public DateTime? Verified { get; set; }

        public bool IsVerified => Verified.HasValue;

        public bool AccountLockedOut { get; set; } = false;
    }
}
