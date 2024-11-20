using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.User;
using Microsoft.AspNetCore.Components;

namespace BT.Admin.Services
{
    public interface IAdminService
    {
        Task<BaseAPIResponseDTO?> CreateUser(NavigationManager navigationManager, NewUserDTO dto);
        Task<List<RoleLiteDTO>?> GetRoles(NavigationManager navigationManager);
        Task<BaseAPIResponseDTO?> AddRoleAsync(NavigationManager navigationManager, AddRoleDTO dto);
        Task<List<BTUserLite>?> GetUsers(NavigationManager navigationManager);
        Task<List<RoleLiteDTO>?> GetRolesForUser(NavigationManager navigationManager, Guid? userId);
        Task<BaseAPIResponseDTO?> UpdateUser(NavigationManager navigationManager, EditUserDTO dto);
    }
}
