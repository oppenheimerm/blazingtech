using BT.Shared.Domain.DTO;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.User;
using Microsoft.AspNetCore.Components;

namespace BT.Admin.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public readonly HttpClient _httpClient;
        readonly IConfiguration _iconfig;
        const string baseUrl = "http://localhost:5000/api/accounts";


        public AccountService(HttpClient httpClient, IConfiguration configuration) :base(httpClient, configuration)
        {
            _httpClient = httpClient;
            _iconfig = configuration;
        }

        public async Task<APIResponJWTDTO> LoginAsync(LoginDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/login", dto);
            var result = await response.Content.ReadFromJsonAsync<APIResponJWTDTO>();
            return result!;
        }

        public async Task<BaseAPIResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/register", dto);
            var result = await response.Content.ReadFromJsonAsync<BaseAPIResponseDTO>();
            return result!;
        }


        /// <summary>
        /// Authentication required
        /// </summary>
        /// <returns></returns>
        public async Task<WeatherForcastDTO[]?> GetWeatherForcast(NavigationManager navigationManager)
        {
            GetProtectedClient();
            var response = await _httpClient.GetAsync($"{baseUrl}/weather")!;
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await GetWeatherForcast(navigationManager);
            }

            return await response.Content.ReadFromJsonAsync<WeatherForcastDTO[]>()!;
        }

        #region Helpers

        /*public void GetProtectedClient()
        {
            if (Constants.JWTToken == "") return;

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.JWTToken);
        }

        static bool CheckIfUnauthroized(HttpResponseMessage httpResposneMessage)
        {
            if (httpResposneMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return true;
            else
                return false;
        }*/

        #endregion
    }
}
