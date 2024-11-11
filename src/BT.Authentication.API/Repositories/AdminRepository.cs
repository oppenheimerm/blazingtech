using BT.Authentication.API.Data;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BT.Authentication.API.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        readonly ApplicationDbContext _appDBContext;
        readonly ILogger<AdminRepository> _logger;

        public AdminRepository(ApplicationDbContext applicationDbContext, ILogger<AdminRepository> logger)
        {
            _appDBContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<BaseAPIResponseDTO> AddRoleAsync(AddRoleDTO dto)
        {
            try
            {
                var findRole = await _appDBContext.Role.FirstOrDefaultAsync(r => r.RoleCode == dto.RoleCode!.ToUpper());
                if (findRole is not null) return new BaseAPIResponseDTO(false, $"Role code: {dto.RoleCode} already in use.");


                var newRole = dto.ToEntity();

                await _appDBContext.Role.AddAsync(newRole);
                await _appDBContext.SaveChangesAsync();
                var msg = $"Failed to locate update member photo.  The passed in member parameter is null. Timestamp: {DateTime.UtcNow}";
                _logger.LogInformation(msg);
                return new BaseAPIResponseDTO(true, $"Role: {newRole.RoleName} with code: {newRole.RoleCode} added to database at {DateTime.UtcNow}");
            }
            catch(Exception err) {
                _logger.LogError(err.ToString());
                return new BaseAPIResponseDTO(false, $"Failed to add role:{dto.RoleName} to database");
            }
        }

        // Authorise
        public async Task<BaseAPIResponseDTO> AddUserToRole(AddUserToRoleDTO dto)
        {

            var role = await _appDBContext.Role.FirstOrDefaultAsync(r => r.RoleCode == dto.RoleCode);
            if (role is null) return new BaseAPIResponseDTO(false, $"The role code: {dto.RoleCode} does not esixt");

            var user = await _appDBContext.ApplicationUser.FirstAsync(u => u.Id == dto.BTUserId);
            if (user is null) return new BaseAPIResponseDTO(false, "User not found.");


            //  Is user already in role?
            var userInRole = await _appDBContext.UserRole.FirstOrDefaultAsync(ur => ur.RoleCode == dto.RoleCode);
            if (userInRole is not null) return new BaseAPIResponseDTO(false, "User already in rolw");

            var newUserRole = new UserRole()
            {
                BTUserId = user.Id,
                RoleCode = role.RoleCode,
                TimeStamp = DateTime.UtcNow
            };
            _appDBContext.UserRole.Add(newUserRole);
            await _appDBContext.SaveChangesAsync();
            return new BaseAPIResponseDTO(true, $"Successuflly added user: {user.Email} to role: {role.RoleName}");
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _appDBContext.Role
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
