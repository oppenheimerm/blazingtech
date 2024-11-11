using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Responses;

namespace BT.Authentication.API.Repositories
{
    public interface IAdminRepository
    {
        Task<List<Role>> GetRoles();
        Task<BaseAPIResponseDTO> AddRoleAsync(AddRoleDTO dto);
        Task<BaseAPIResponseDTO> AddUserToRole(AddUserToRoleDTO dto);
    }
}
