using BT.Shared;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO;

namespace BT.Authentication.API.Interface
{
    public interface IUser
    {
        Task<Response> Register(RegisterDTO registerDTO);
        Task<Response> ConfirmEmailAddress(string email, string code);
        Task<BTUser> GetUserByEmail(string email);
        Task<GetUserDTO> GetUserByEmailExternal(string email);
    }
}
