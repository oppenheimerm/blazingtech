using Microsoft.EntityFrameworkCore;
using BT.Authentication.API.Data;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.User;
using BT.Authentication.API.Helpers;
using System.Security.Cryptography;


namespace BT.Authentication.API.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        readonly ApplicationDbContext _appDBContext;
        readonly ILogger<AdminRepository> _logger;
        readonly IConfiguration _configuration;

        public AdminRepository(ApplicationDbContext applicationDbContext, ILogger<AdminRepository> logger,
            IConfiguration configuration)
        {
            _appDBContext = applicationDbContext;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<BaseAPIResponseDTO> CreateUserAsync(NewUserDTO dto)
        {
            try
            {
                var emailInUse = await EmailInUser(dto.Email.ToLower());
                if(emailInUse == true) return new BaseAPIResponseDTO(false, "Email already in use");
                                
                //  Validate password
                var passwordValid = AccountHelpers.ValidatePassword(dto.Password, int.Parse(_configuration["ApplicationSettings:MinimumPasswordLength"]!));
                if (!passwordValid) return new(false, $"Passwords must be a minimum of {int.Parse(_configuration["ApplicationSettings:MinimumPasswordLength"]!)}, contain at least one upper case character, one lower case character, at least one number and at least one non alpha numeric character.");


                var newUser = dto.ToEntity();

                newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                newUser.VerificationToken = await GenerateVerificationTokenAsync();

                _appDBContext.ApplicationUser.Add(newUser);
                await _appDBContext.SaveChangesAsync();

                _logger.LogInformation($"User with Id: {newUser.Id} successfully created at: {DateTime.UtcNow}");
                return new BaseAPIResponseDTO(true, "Please confirm email address.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Operation failed while creating user with error: {ex.ToString()}. Timestamp : {DateTime.UtcNow}");
                return new BaseAPIResponseDTO(false, "Unable to create user.");
            }
            

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
            try
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

                _logger.LogInformation($"User with Id: { dto.BTUserId } successfully added to role: { newUserRole.RoleCode } created at: {DateTime.UtcNow}");
                return new BaseAPIResponseDTO(true, $"Successuflly added user: {user.Email} to role: {role.RoleName}");
            }
            catch (Exception ex) {
                _logger.LogError($"Operation failed to add user to role.  Error: { ex.ToString()}. Timestamp : {DateTime.UtcNow}");
                return new BaseAPIResponseDTO(false, "Failed to add user to role.");
            }

            
        }

        public async Task<List<RoleLiteDTO>> GetRoles()
        {
            return await _appDBContext.Role
                .AsNoTracking()
                .Select( r => new RoleLiteDTO()
                {
                    RoleCode = r.RoleCode,
                    RoleName = r.RoleName,
                    Description = r.Description

                }).ToListAsync();
        }

        public async Task<List<RoleLiteDTO>?> GetUserRolesAsync(Guid Id)
        {
            /*return await _appDBContext.UserRole
                .Where(u => u.BTUserId == Id)
                .Include(r => r.Role)
                .AsNoTracking()
                .ToListAsync();*/

            /*var query = from userRole in _appDBContext.UserRole
                        where userRole.BTUserId == Id
                        select new RoleLiteDTO
                        {
                            RoleCode = userRole.RoleCode,
                            RoleName = userRole.Role.RoleName,
                            Description = userRole.Role
                        };

           var results = query.OrderBy(r => r.RoleCode).AsQueryable();
            return results;*&*/

            var query = await _appDBContext.UserRole
                .Where(u => u.BTUserId == Id)
                .Include(r => r.Role)
                .AsNoTracking()
                .Select(r => new RoleLiteDTO
                {
                    RoleCode = r.RoleCode,
                    RoleName = r.Role!.RoleName,
                    Description = r.Role.Description
                }).ToListAsync();


            var results = query;
            return query;
        }

        public IQueryable<BTUserLite> GetUsers()
        {
            /*return await _appDBContext.ApplicationUser
                .AsNoTracking()
                .ToListAsync();*/

            var table = from usr in _appDBContext.ApplicationUser
                        select new BTUserLite
                        {
                            Id = usr.Id,
                            JoinDate = usr.JoinDate,
                            FirstName = usr.FirstName,
                            LasttName = usr.LasttName,
                            Email = usr.Email,
                            Address = usr.Address,
                            ProfilePicture = usr.ProfilePicture,
                            Mobile = usr.Mobile,
                            Verified = usr.Verified,
                            AccountLockedOut = usr.AccountLockedOut
                        };

            return table
                .OrderBy(u => u.FirstName)
                .AsQueryable();
        }

        public async Task<BaseAPIResponseDTO> UpdateUserAsync(EditUserDTO dto)
        {

            try {
                var user = await _appDBContext.ApplicationUser.FirstOrDefaultAsync(u => u.Id == dto.Id);
                if (user is null)
                    return new BaseAPIResponseDTO() { Message = "User id is required.", Success = false };

                if(dto.Email != user.Email)
                {
                    var emailInUse = await EmailInUser(dto.Email);
                    if (emailInUse == true)
                        return new BaseAPIResponseDTO() { Message = $"The email: {dto.Email} is already in user", Success = false };
                }
                

                var currentUserRoles = await _appDBContext.UserRole
                    .Where(u => u.BTUserId == dto.Id)
                    .ToListAsync();

                //  restricted fields to edit
                //  Can't change user Id
                user.FirstName = dto.FirstName;
                user.LasttName = dto.LasttName;
                user.Email = dto.Email;
                user.Address = dto.Address;
                user.Mobile = dto.Mobile;
                user.AccountLockedOut = dto.AccountLockedOut;                

                //  Delete old
                if(currentUserRoles is not null)
                {
                    if(currentUserRoles.Count >= 1) {
                        await DeleteOldRoles(currentUserRoles);
                    }
                }
                


                _appDBContext.Entry(user).State = EntityState.Modified;
                _appDBContext.ApplicationUser.Update(user);
                await _appDBContext.SaveChangesAsync();

                // add new roles
                if(dto.UserRoles != null)
                {
                    if(dto.UserRoles.Count >= 1)
                    {
                        await UpdateUserRolesAsync(dto.UserRoles, user.Id);
                    }
                }
                

                _logger.LogInformation($"USer with Id: {user.Id}, was updated at: {DateTime.UtcNow}");

                return new BaseAPIResponseDTO() { Message = "Successfully updated user", Success = true };
            }
            catch (Exception ex) {
                _logger.LogError($"Failed to update user. Exception: {ex.ToString()} Timestamp : {DateTime.UtcNow}");
                return new BaseAPIResponseDTO() { Success = false, Message = "Failed to update user." };
            }            
        }

        #region Helpers


        async Task<bool> EmailInUser(string email)
        {
            var query = await _appDBContext.ApplicationUser.FirstOrDefaultAsync(u => u.Email == email.ToLower());
            return query is null ? false : true;
        }

        async Task DeleteOldRoles(List<UserRole> oldRoles)
        {
            try {
                _appDBContext.UserRole.RemoveRange(oldRoles);
                await _appDBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete user's old roles. Exception: {ex.ToString()} Timestamp : {DateTime.UtcNow}");
            }
        }


        public async Task UpdateUserRolesAsync(List<RoleLiteDTO>? newRoles, Guid userId)
        {
            if(newRoles != null)
            {
                var query = from role in newRoles
                            select new UserRole
                            {
                                BTUserId = userId,
                                RoleCode = role.RoleCode,
                                TimeStamp = DateTime.UtcNow
                            };

                await _appDBContext.UserRole.AddRangeAsync(query.ToList());
                await _appDBContext.SaveChangesAsync();
            }
        }

        private async Task<string> GenerateVerificationTokenAsync()
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            // ensure token is unique by checking against db
            //var tokenIsUnique =  !_appDBContext.ApplicationUsers.AnyAsync(x => x.VerificationToken == token);
            var tokenIsUnique = !await _appDBContext.ApplicationUser.AnyAsync(x => x.VerificationToken == token);
            if (!tokenIsUnique)
                return await GenerateVerificationTokenAsync();

            return token;
        }


        #endregion
    }
}
