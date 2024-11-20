using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Category;
using Microsoft.AspNetCore.Components;

namespace BT.Admin.Services
{
    public class ProductService : BaseService, IProductService
    {
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;
        const string baseUrlProduct = "http://localhost:5001/api/admin";
        const string baseUrlCategory = "http://localhost:5001/api/Categories";

        public ProductService(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<CategoryDTO>?> GetCategories(NavigationManager navigationManager)
        {
            GetProtectedClient();
            var response = await _httpClient.GetAsync($"{baseUrlCategory}")!;
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await GetCategories(navigationManager);
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<CategoryDTO>>();

        }
    }
}
