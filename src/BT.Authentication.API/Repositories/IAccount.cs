using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Responses;

namespace BT.Authentication.API.Repositories
{
    public interface IAccount
    {
        Task<BTUser?> GetUser(Guid Id);
        Task<BTUser?> GetUser(string email);
        Task<BaseAPIResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<APIResponJWTDTO> LoginAsync(LoginDTO dto);
    }
}
