using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.User;
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


        public async Task<BaseAPIResponseDTO?> CreateUser(NavigationManager navigationManager, NewUserDTO dto)
        {
            GetProtectedClient();
            var paylod = JsonSerializer.Serialize(dto);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/create-user");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(paylod, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(request);
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await CreateUser(navigationManager, dto);
            }

            return await response.Content.ReadFromJsonAsync<BaseAPIResponseDTO?>();
        }

        public async Task<BaseAPIResponseDTO?> UpdateUser(NavigationManager navigationManager, EditUserDTO dto)
        {
            GetProtectedClient();
            var paylod = JsonSerializer.Serialize(dto);
            var request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/update-user");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(paylod, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(request);
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await UpdateUser(navigationManager, dto);
            }

            return await response.Content.ReadFromJsonAsync<BaseAPIResponseDTO?>();
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


        public async Task<List<RoleLiteDTO>?> GetRoles(NavigationManager navigationManager)
        {
            GetProtectedClient();
            var response = await _httpClient.GetAsync($"{baseUrl}/roles")!;
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await GetRoles(navigationManager);
            }

            return await response.Content.ReadFromJsonAsync<List<RoleLiteDTO>>();

        }

        public async Task<List<BTUserLite>?> GetUsers(NavigationManager navigationManager)
        {
            GetProtectedClient();
            var response = await _httpClient.GetAsync($"{baseUrl}/users")!;
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await GetUsers(navigationManager);
            }

            return await response.Content.ReadFromJsonAsync<List<BTUserLite>>();
        }

        public async Task<List<RoleLiteDTO>?> GetRolesForUser(NavigationManager navigationManager, Guid? userId)
        {
            if (!userId.HasValue)
            {
                return null!;
            }

            GetProtectedClient();
            var response = await _httpClient.GetAsync($"{baseUrl}/user-roles?Id={userId}")!;
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await GetRolesForUser(navigationManager, userId);
            }

            return await response.Content.ReadFromJsonAsync<List<RoleLiteDTO>>();           

        }
    }
}
