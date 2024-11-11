using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Responses;
using Microsoft.AspNetCore.Components;

namespace BT.Admin.Services
{
    public interface IAdminService
    {
        Task<List<Role>?> GetRoles(NavigationManager navigationManager);
        Task<BaseAPIResponseDTO?> AddRoleAsync(NavigationManager navigationManager, AddRoleDTO dto);
    }
}
