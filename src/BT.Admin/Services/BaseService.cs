using BT.Admin.AuthState;
using BT.Shared.Domain.DTO;
using BT.Shared.Domain.DTO.Responses;
using Microsoft.AspNetCore.Components;


namespace BT.Admin.Services
{
    public class BaseService
    {
        readonly HttpClient _httpClient;
        readonly IConfiguration _iconfig;

        public BaseService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _iconfig = configuration;
        }


        public async Task GetRefreshToken(NavigationManager navigationManager)
        {
            if (string.IsNullOrEmpty(Constants.JWTToken))
            { 
                navigationManager.NavigateTo("/Account/Login", true);
                return;
            }
            else
            {

                var baseUrl = _iconfig["ApplicationSettings:AccountAPIBaseURL"];
                var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/refresh-token", new UserSession() { JWTToken = Constants.JWTToken });
                    var result = await response.Content.ReadFromJsonAsync<APIResponJWTDTO>();
                Constants.JWTToken = result!.JWTToken;

                return;
            }


        }

        public void GetProtectedClient()
        {
            if (Constants.JWTToken == "") return;

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.JWTToken);
        }

        public static bool CheckIfUnauthroized(HttpResponseMessage httpResposneMessage)
        {
            if (httpResposneMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return true;
            else
                return false;
        }
    }
}
