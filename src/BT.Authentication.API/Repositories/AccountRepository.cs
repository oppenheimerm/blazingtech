﻿using BT.Authentication.API.Data;
using BT.Authentication.API.DTO;
using BT.Authentication.API.Helpers;
using BT.Authentication.API.Services.AuthService;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BT.Authentication.API.Repositories
{
    public class AccountRepository : IAccount
    {
        readonly ApplicationDbContext _appDBContext;
        readonly IConfiguration _configuration;
        readonly IJWTUtilities _jWTUtilities;

        public AccountRepository(ApplicationDbContext applicationDbContext, IConfiguration configuration, IJWTUtilities jWTUtilities)
        {
            _appDBContext = applicationDbContext;
            _configuration = configuration;
            _jWTUtilities = jWTUtilities;
        }

        public async Task<BaseAPIResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            var findUser = await GetUser(dto.Email);
            if (findUser is not null) return new BaseAPIResponseDTO(false, "Email already in user");

            //  Validate password
            var passwordValid = AccountHelpers.ValidatePassword(dto.Password, int.Parse(_configuration["ApplicationSettings:MinimumPasswordLength"]!));
            if (!passwordValid) return new (false, $"Passwords must be a minimum of {int.Parse(_configuration["ApplicationSettings:MinimumPasswordLength"]!)}, contain at least one upper case character, one lower case character, at least one number and at least one non alpha numeric character.");

            var newUser = dto.ToEntity();


            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            newUser.VerificationToken = await GenerateVerificationTokenAsync();
            
            _appDBContext.ApplicationUser.Add(newUser);
            await _appDBContext.SaveChangesAsync();
            return new BaseAPIResponseDTO (true, "Please confirm email address.");

        }

        public async Task<APIResponJWTDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _appDBContext.ApplicationUser
                .Where(u => u.Email == dto.Email)
                .Include(u => u.Roles)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user is null) return new APIResponJWTDTO(false, "Account not found.", string.Empty);

            //  Account verified?
            if (!user.IsVerified) return new APIResponJWTDTO(false, "Please check you email and verify your account.", string.Empty);

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return new APIResponJWTDTO(false, "Email or password error", string.Empty);

            //  TODO  VERIFY this include the role data

            /*var roles = await _appDBContext.UserRole
                .Where(u => u.BTUserId == user.Id)
                .Include(r => r.Role)
                .AsNoTracking()
                .ToListAsync();*/


            string jwtToken = _jWTUtilities.GenerateToken(user);
            return new APIResponJWTDTO(true, "Login successfull", jwtToken);

        }

        //Authorize
        public async Task<BaseAPIResponseDTO> CreateRoleAsync(CreateRoleDTO dto) {

            var roleCodeExist = await _appDBContext.Role.AnyAsync(r => r.RoleCode == dto.RoleCode!.ToLower());
            if (roleCodeExist)
                return new BaseAPIResponseDTO(false, $"Existing role with the code: {dto.RoleCode} already exist.");

            var newRole = dto.ToEntity();
            _appDBContext.Role.Add(newRole);
            await _appDBContext.SaveChangesAsync();
            return new BaseAPIResponseDTO(true, $"Role: {dto.RoleName}, successfully added.");
        }

        // Authorise
        public async Task<BaseAPIResponseDTO> AddUserToRole(AddUserToRoleDTO dto) {

            var role = await _appDBContext.Role.FirstOrDefaultAsync(r => r.RoleCode == dto.RoleCode);
            if (role is null) return new BaseAPIResponseDTO(false, $"The role code: {dto.RoleCode} does not esixt");

            var user = await _appDBContext.ApplicationUser.FirstAsync(u => u.Id == dto.BTUserId);
            if (user is null) return new BaseAPIResponseDTO(false, "User not found.");


            //  Is user already in role?
            var userInRole = await _appDBContext.UserRole.FirstOrDefaultAsync(ur => ur.RoleCode == dto.RoleCode);
            if (userInRole is not null) return new BaseAPIResponseDTO(false, "User already in rolw");

            var newUserRole = new UserRole() { 
                BTUserId = user.Id,
                RoleCode = role.RoleCode,
                TimeStamp = DateTime.UtcNow
            };
            _appDBContext.UserRole.Add(newUserRole);
            await _appDBContext.SaveChangesAsync();
            return new BaseAPIResponseDTO(true, $"Successuflly added user: {user.Email} to role: {role.RoleName}");
        }

        public async Task<APIResponJWTDTO> RefreshTokenAsync(UserSession userSession)
        {
            AppUserClaimsDTO appUserClaims = _jWTUtilities.DecryptToken(userSession.JWTToken);
            if (appUserClaims is null) return new APIResponJWTDTO(false, "Invalid token.");

            //  As this is async, can't we we check(always) that the user roles
            //  are the lastes as indicated by the database?  i.e. UserRole entity.
            var user = await GetUserIncludeRolesAsync(appUserClaims.Id!.Value);

            if (user is null) return new APIResponJWTDTO(false, "Invalid token for user.");

            string newToken = _jWTUtilities.GenerateToken(new BTUser()
            {
                FirstName = appUserClaims.FirstName,
                Email = appUserClaims.Email,
                Roles = user.Roles
            });

            return new APIResponJWTDTO(true, "Token refreshed successfully.", newToken);
        }


        public async Task<BTUser?> GetUser(string email) => await _appDBContext.ApplicationUser.FirstOrDefaultAsync(e => e.Email == email);
        public async Task<BTUser?> GetUser(Guid Id) => await _appDBContext.ApplicationUser.FirstOrDefaultAsync(u => u.Id == Id);

        /// <summary>
        /// Speciality function for for getting an instance of <see cref="BTUser"/> with the roles of said user
        /// up to date
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BTUser?> GetUserIncludeRolesAsync(Guid Id)
        {
            var user = await _appDBContext.ApplicationUser
                .Where(u => u.Id == Id)
                .Include( role => role.Roles)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return user!;
        }
        #region Helpers


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