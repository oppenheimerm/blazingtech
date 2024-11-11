using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Responses;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace BT.Admin.Services
{
    public class AdminService : BaseService, IAdminService
    {
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;
        const string baseUrl = "http://localhost:5000/api/admin";

        public AdminService(HttpClient httpClient, IConfiguration configuration):base(httpClient, configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<BaseAPIResponseDTO?> AddRoleAsync(NavigationManager navigationManager, AddRoleDTO dto)
        {
            GetProtectedClient();
            var role = JsonSerializer.Serialize(dto);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/add-role");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(role, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            var response = await _httpClient.SendAsync(request);
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await AddRoleAsync(navigationManager, dto);
            }

            return await response.Content.ReadFromJsonAsync<BaseAPIResponseDTO?>();
        }

        /*public async Task<WeatherForcastDTO[]?> GetWeatherForcast(NavigationManager navigationManager)
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
        }*/


        public async Task<List<Role>?> GetRoles(NavigationManager navigationManager)
        {
            GetProtectedClient();
            var response = await _httpClient.GetAsync($"{baseUrl}/roles")!;
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await GetRoles(navigationManager);
            }

            return await response.Content.ReadFromJsonAsync<List<Role>>();

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
