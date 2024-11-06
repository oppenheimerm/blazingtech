
namespace BT.Shared.Domain.DTO
{
    /// <summary>
    /// striped down class of <see cref="BTUser"/> with minimal properties
    /// </summary>
    public class GetUserDTO
    {
        public string? FirstName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Role { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Address { get; set; }
        public DateTime? RegisterDate { get; set; }
    }
}
