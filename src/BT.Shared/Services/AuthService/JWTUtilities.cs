
using BT.Shared.Domain.DTO;
using BT.Shared.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BT.Shared.Domain.DTO.Responses;

namespace BT.Shared.Services.AuthService
{
    public class JWTUtilities : IJWTUtilities
    {
        public string GenerateToken(BTUser user, string AuthKey, string AuthIssuer, string AuthAudience)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthKey));
            var credentrials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[] {
                //var Id = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier);
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName!),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            if (user.Roles is not null)
            {
                foreach (var role in user.Roles)
                {
                    userClaims.Append(
                        new Claim(ClaimTypes.Role, role.RoleName!));
                }
            }

            var token = new JwtSecurityToken(
                issuer: AuthIssuer,
                audience: AuthAudience,
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentrials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public AppUserClaimsDTO DecryptToken(string jwtToken)
        {
            try
            {
                if (string.IsNullOrEmpty(jwtToken)) return new AppUserClaimsDTO();

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);
                List<Role>? rolesCollection = [];

                var Id = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier);
                var firstName = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
                var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email);


                var _roles = token.Claims.Where(_ => _.Type == ClaimTypes.Role).ToList();
                if (_roles is not null)
                {
                    foreach (var role in _roles)
                    {
                        var _newRole = new Role()
                        {
                            RoleName = role.Value
                        };
                        rolesCollection.Add(_newRole);
                    }
                }

                return new AppUserClaimsDTO(Guid.Parse(Id!.Value), firstName!.Value, email!.Value, rolesCollection!);
            }
            catch
            {
                return null!;
            }
        }

        public APIResponJWTDTO RefreshToken(UserSession userSession, string AuthKey, string AuthIssuer, string AuthAudience)
        {
            AppUserClaimsDTO appUserClaims = DecryptToken(userSession.JWTToken);
            if (appUserClaims is null) return new APIResponJWTDTO(false, "Invalid token.");

            string newToken = GenerateToken(new BTUser()
            {
                Id = appUserClaims.Id!.Value,
                FirstName = appUserClaims.FirstName,
                Email = appUserClaims.Email,
                Roles = appUserClaims.Roles
            }, AuthKey, AuthIssuer, AuthAudience);

            return new APIResponJWTDTO(true, "Token refreshed successfully.", newToken);
        }
    }
}
