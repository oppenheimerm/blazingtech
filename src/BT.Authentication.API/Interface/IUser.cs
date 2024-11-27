using BT.Shared;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.User;

namespace BT.Authentication.API.Interface
{
    public interface IUser
    {
        Task<BaseAPIResponseDTO> Register(RegisterDTO registerDTO);
        Task<BaseAPIResponseDTO> ConfirmEmailAddress(string email, string code);
        Task<BTUser> GetUserByEmail(string email);
        Task<GetUserDTO> GetUserByEmailExternal(string email);
    }
}
