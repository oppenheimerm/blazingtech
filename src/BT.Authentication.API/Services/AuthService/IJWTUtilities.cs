using BT.Authentication.API.DTO;
using BT.Shared.Domain.DTO;
using BT.Shared.Domain;

namespace BT.Authentication.API.Services.AuthService
{
    public interface IJWTUtilities
    {
        public string GenerateToken(BTUser user);
        public AppUserClaimsDTO DecryptToken(string jwtToken);
        public APIResponJWTDTO RefreshToken(UserSession userSession);
    }
}
