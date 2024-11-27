using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BT.Shared.Domain.DTO.Category;
using BT.Shared.Domain.DTO.Product;
using BT.Shared.Domain.DTO.Responses;
using BT.Admin.Helpers;
using BT.Shared.Domain;

namespace BT.Admin.Services
{
    public class ProductService : BaseService, IProductService
    {
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;
        const string baseUrlProduct = "http://localhost:5001/api/products";
        const string baseUrlCategory = "http://localhost:5001/api/categories";

        public ProductService(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<ProductDTO>?> GetProducts(NavigationManager navigationManager)
        {
            GetProtectedClient();
            var response = await _httpClient.GetAsync($"{baseUrlProduct}/all")!;
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await GetProducts(navigationManager);
            }
            var items = await response.Content.ReadFromJsonAsync<List<ProductDTO>?>();
            return items;

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

        public async Task<BaseAPIResponseDTO?> GetNewProductImageNameAsync(NavigationManager navigationManager, string fileExtension)
        {
            GetProtectedClient();
            var response = await _httpClient.GetAsync($"{baseUrlProduct}/new-image-filename?ext={fileExtension}&numberOftries={0}" )!;
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await GetNewProductImageNameAsync(navigationManager, fileExtension);
            }

            return await response.Content.ReadFromJsonAsync<BaseAPIResponseDTO>();
        }

        public async Task<APIResponseProduct?> CreateProductAsync(NavigationManager navigationManager, AddProductDTO dto)
        {
            GetProtectedClient();
            // Api/Products/create needs a type of: AddProductEntityDTO
            var paylod = JsonSerializer.Serialize(dto.ToEntity());
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrlProduct}/create");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(paylod, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(request);
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await CreateProductAsync(navigationManager, dto);
            }

            return await response.Content.ReadFromJsonAsync<APIResponseProduct>();

        }

        public async Task<APIResponseCategory?> CreateCategorytAsync(NavigationManager navigationManager, CategoryDTO dto)
        {
            GetProtectedClient();
            var paylod = JsonSerializer.Serialize(dto);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrlCategory}/create");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(paylod, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(request);
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await CreateCategorytAsync(navigationManager, dto);
            }

            return await response.Content.ReadFromJsonAsync<APIResponseCategory>();

        }

        public async Task<APIResponseProductImage?> CreateProductImageDbEntity(NavigationManager navigationManager, CreatePhotoDTO dto)
        {

            GetProtectedClient();

            var paylod = JsonSerializer.Serialize(dto);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrlProduct}/create-product-image");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(paylod, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            var response = await _httpClient.SendAsync(request);
            bool check = CheckIfUnauthroized(response);
            if (check)
            {
                await GetRefreshToken(navigationManager);
                return await CreateProductImageDbEntity(navigationManager, dto);
            }

            return await response.Content.ReadFromJsonAsync<APIResponseProductImage>();

        }
    }
}
