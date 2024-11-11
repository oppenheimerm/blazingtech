
using BT.Shared.Domain;
using BT.Shared.Domain.DTO;
using BT.Shared.Domain.DTO.Responses;

namespace BT.Shared.Services.AuthService
{
    public interface IJWTUtilities
    {
        public string GenerateToken(BTUser user, string AuthKey, string AuthIssuer, string AuthAudience);
        public AppUserClaimsDTO DecryptToken(string jwtToken);
        public APIResponJWTDTO RefreshToken(UserSession userSession, string AuthKey, string AuthIssuer, string AuthAudience);
    }
}
