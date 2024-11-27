using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain;
using System.Linq.Expressions;

namespace BT.Products.API.Repositories
{
    public interface ICategoryRepository
    {
        Task<APIResponseCategory> CreateAsync(Category entity);
        Task<BaseAPIResponseDTO> DeleteAsync(Category entity);
        Task<Category> FindByIdAsync(string categoryCode);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByAsync(Expression<Func<Category, bool>> predicate);
        Task<BaseAPIResponseDTO> UdateAsync(Category entity);
    }
}
