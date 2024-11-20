using BT.Shared.Domain.DTO.Category;
using Microsoft.AspNetCore.Components;

namespace BT.Admin.Services
{
    public interface IProductService
    {
        Task<IEnumerable<CategoryDTO>?> GetCategories(NavigationManager navigationManager);
    }
}
