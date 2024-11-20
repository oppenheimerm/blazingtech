using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.User;

namespace BT.Authentication.API.Repositories
{
    public interface IAdminRepository
    {
        Task<BaseAPIResponseDTO> CreateUserAsync(NewUserDTO dto);
        Task<List<RoleLiteDTO>> GetRoles();
        Task<BaseAPIResponseDTO> AddRoleAsync(AddRoleDTO dto);
        Task<BaseAPIResponseDTO> AddUserToRole(AddUserToRoleDTO dto);
        Task<List<RoleLiteDTO>?> GetUserRolesAsync(Guid Id);
        /// <summary>
        /// Get all users <see cref="BTUser"/>.
        /// </summary>
        /// <returns></returns>
        //Task<List<BTUser>> GetUsers();
        IQueryable<BTUserLite> GetUsers();
        Task<BaseAPIResponseDTO> UpdateUserAsync(EditUserDTO dto);
    }
}
