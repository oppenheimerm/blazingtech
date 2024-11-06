
namespace BT.Shared.Domain.DTO
{
    public record AppUserClaimsDTO(
        Guid? Id = null!,
        string FirstName = null!,
        string Email = null!,
        List<Role>? Roles = null!
        );
}
