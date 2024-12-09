using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain;
using System.Linq.Expressions;
using BT.Shared.Domain.DTO.Product;

namespace BT.Products.API.Repositories
{
    public interface IProductRepository
    {
        Task<APIResponseProduct> CreateAsync(Product entity);
        Task<APIResponseProductImage> CreatePhotoDbEntityAsync(ProductImage entity);
        Task<BaseAPIResponseDTO> DeleteAsync(Product entity);
        Task<Product> FindByIdAsync(int Id);
        Task<bool> ImageFileExistAsync(string imageName);
        Task<IEnumerable<Product>> GetAllAsync();
        IQueryable<ProductDTO>? GetAllWithImages();
        Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate);
        //Task<BaseAPIResponseDTO> UdateAsync(Product entity);
        Task<APIResponseProduct> UdateProductAsync(EditProductDTO dto);


    }
}
