using BT.Shared.Domain;
using BT.Shared.Domain.DTO;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.User;

namespace BT.Authentication.API.Repositories
{
    public interface IAccount
    {
        Task<BTUser?> GetUser(Guid Id);
        Task<BTUser?> GetUser(string email);
        Task<BaseAPIResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<VerifyEmailDTO> VerifyEmail(string token);
        Task<APIResponJWTDTO> LoginAsync(LoginDTO dto);
        Task<APIResponJWTDTO> RefreshTokenAsync(UserSession userSession);
    }
}
