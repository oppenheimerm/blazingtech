

using System.Linq.Expressions;

namespace BT.Shared.Interfaces
{
    public interface IGenericInterface<T> where T : class
    {
        Task<Response> CreateAsync(T entity);
        Task<Response> UdateAsync(T entity);
        Task<Response> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindByIdAsync(int Id);
        Task<T> FindByIdAsync(Guid Id);
        Task<T> FindByIdAsync(string Id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
