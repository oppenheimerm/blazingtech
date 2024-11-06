using BT.Authentication.API.DTO;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO;

namespace BT.Authentication.API.Repositories
{
    public interface IAccount
    {
        Task<BTUser?> GetUser(Guid Id);
        Task<BTUser?> GetUser(string email);
        Task<BaseAPIResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<APIResponJWTDTO> LoginAsync(LoginDTO dto);
        Task<BaseAPIResponseDTO> CreateRoleAsync(CreateRoleDTO dto);
        Task<BaseAPIResponseDTO> AddUserToRole(AddUserToRoleDTO dto);
    }
}
