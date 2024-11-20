using BT.Shared.Domain.DTO;
using BT.Shared.Services.AuthService;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BT.Admin.AuthState
{
    // AuthenticationStateProvider Provides information about the authentication state of the current user.
    //  https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-8.0
    //  https://learn.microsoft.com/en-us/aspnet/core/blazor/security/authentication-state?view=aspnetcore-8.0&pivots=server#abstract-authenticationstateprovider-class
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        readonly IJWTUtilities _jwtUtilities;
        public CustomAuthenticationStateProvider(IJWTUtilities jWTUtilities)
        {
            _jwtUtilities = jWTUtilities;
        }

        readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                //  User is anonymous / not logged in or authenticated
                if (string.IsNullOrEmpty(Constants.JWTToken))
                    return await Task.FromResult(new AuthenticationState(anonymous));

                var getUserClaims = _jwtUtilities.DecryptToken(Constants.JWTToken);
                if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymous));

                //  Create a claims principal
                var claimsPrincipal = SetClaimsPrincipal(getUserClaims);
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

        public static ClaimsPrincipal SetClaimsPrincipal(AppUserClaimsDTO claims)
        {
            if (claims.Email is null) return new ClaimsPrincipal();

            var userClaims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, claims.Id.ToString()!),
                new Claim(ClaimTypes.Name, claims.FirstName!),
                new Claim(ClaimTypes.Email, claims.Email!),
            };

            if (claims.Roles is not null)
            {
                foreach (var role in claims.Roles)
                {
                    userClaims.Add(
                        new Claim(ClaimTypes.Role, role.RoleCode!));
                }
            }


            return new ClaimsPrincipal(
                new ClaimsIdentity(userClaims, "JwtAuth"
                ));
        }


        public void UpdateAuthenticatedState(string jwtToken)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if (!string.IsNullOrEmpty(jwtToken))
            {
                Constants.JWTToken = jwtToken;
                var getUserClaims = _jwtUtilities.DecryptToken(jwtToken);
                claimsPrincipal = SetClaimsPrincipal(getUserClaims);
            }
            else
            {
                Constants.JWTToken = null!;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
