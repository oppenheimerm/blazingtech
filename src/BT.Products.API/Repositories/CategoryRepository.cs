using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BT.Products.API.Data;
using BT.Shared.APIServiceLogs;
using BT.Shared.Domain;
using BT.Shared;
using BT.Shared.Domain.DTO.Responses;
using BT.Products.API.Domain;

namespace BT.Products.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        ProductDataContext _context { get; set; }

        public CategoryRepository(ProductDataContext context)
        {
            _context = context;
        }

        public async Task<APIResponseCategory> CreateAsync(Category entity)
        {
            try
            {
                //  Validation if exist
                var categoryExist = await GetByAsync(_ => _.Id!.Equals(entity.Id));
                if (categoryExist is not null && !string.IsNullOrEmpty(categoryExist.Title))
                    return new APIResponseCategory(false, $"{entity.Title} already exist");

                var item = _context.Category.Add(entity).Entity;
                await _context.SaveChangesAsync();
                if (item is not null && !string.IsNullOrEmpty(item.Id))
                {
                    return new APIResponseCategory(true, $"{entity.Title} added to databast at {DateTime.UtcNow.ToString()}", item.ToEntity());
                }
                else
                {
                    return new APIResponseCategory(false, $"{AppConstants.CreateDatabaseEntityFailed} {entity.Title} {DateTime.UtcNow.ToString()}");
                }
            }
            catch (Exception ex)
            {
                //  Log original exception
                LogException.LogExceptions(ex);

                // Display friendly message to client
                return new APIResponseCategory(false, AppConstants.CreateDatabaseEntityFailed + " " + DateTime.UtcNow.ToString());
            }
        }

        public async Task<BaseAPIResponseDTO> DeleteAsync(Category entity)
        {
            if (entity.Id is not null)
            {
                try
                {
                    var item = await FindByIdAsync(entity.Id);
                    if (item is null)
                    {
                        return new BaseAPIResponseDTO(false, $"{entity.Id} not found");
                    }

                    // Remove entity
                    _context.Category.Remove(item);
                    await _context.SaveChangesAsync();

                    return new BaseAPIResponseDTO(true, $"{AppConstants.DeleteDatabaseEntitySuccess} {entity.Title} {DateTime.UtcNow.ToString()}");
                }
                catch (Exception ex)
                {
                    //  Log original exception
                    LogException.LogExceptions(ex);

                    // Display friendly message to client
                    return new BaseAPIResponseDTO(false, AppConstants.DeleteDatabaseEntityFailed + " " + DateTime.UtcNow.ToString());
                }
            }
            else {
                return new BaseAPIResponseDTO(false, AppConstants.ParamsMissingError_Id + " " + DateTime.UtcNow.ToString());
            }
        }

        public async Task<Category> FindByIdAsync(string categoryCode)
        {
            try
            {
                var item = await _context.Category.FindAsync(categoryCode);
                return item is not null ? item : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception(AppConstants.ErrorRetrievingEntity);
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                var items = await _context.Category.AsNoTracking().ToListAsync();
                return items is not null ? items : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception(AppConstants.ErrorRetrievingEntity);
            }
        }

        public async Task<Category> GetByAsync(Expression<Func<Category, bool>> predicate)
        {
            try
            {
                var items = await _context.Category.Where(predicate).FirstOrDefaultAsync()!;
                return items is not null ? items : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception(AppConstants.ErrorRetrievingEntity);
            }

        }

        public async Task<BaseAPIResponseDTO> UdateAsync(Category entity)
        {

            if(entity.Id is not null)
            {
                try
                {
                    var item = await FindByIdAsync(entity.Id!);
                    if (item is null)
                        return new BaseAPIResponseDTO(false, $"{entity.Title} Not found");

                    _context.Category.Entry(item).State = EntityState.Deleted;
                    _context.Category.Update(entity);
                    await _context.SaveChangesAsync();
                    return new BaseAPIResponseDTO(true, $"{entity.Title} updated. {DateTime.UtcNow.ToString()}");

                }
                catch (Exception ex)
                {

                    LogException.LogExceptions(ex);
                    return new BaseAPIResponseDTO(false, AppConstants.UpdateDatabaseEntityFailed);

                }
            }
            else
            {
                return new BaseAPIResponseDTO(false, AppConstants.ParamsMissingError_Id);
            }
        }

    }
}
