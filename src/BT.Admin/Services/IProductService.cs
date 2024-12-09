using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Category;
using BT.Shared.Domain.DTO.Product;
using BT.Shared.Domain.DTO.Responses;
using Microsoft.AspNetCore.Components;

namespace BT.Admin.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>?> GetProducts(NavigationManager navigationManager);
        Task<IEnumerable<CategoryDTO>?> GetCategories(NavigationManager navigationManager);
        Task<BaseAPIResponseDTO?> GetNewProductImageNameAsync(NavigationManager navigationManager, string fileExtension);
        Task<APIResponseProductImage?> CreateProductImageDbEntity(NavigationManager navigationManager, CreatePhotoDTO dto);
        Task<APIResponseProduct?> CreateProductAsync(NavigationManager navigationManager, AddProductDTO dto);
        //  Remove
        //Task<APIResponseAlbum?> CreateProductAlbumDbEntity(NavigationManager navigationManager, CreateAlbumDTO dto);
        Task<APIResponseCategory?> CreateCategorytAsync(NavigationManager navigationManager, CategoryDTO dto);
        Task<APIResponseProduct?> UpdateProduct(NavigationManager navigationManager, EditProductDTO dto);
    }
}
