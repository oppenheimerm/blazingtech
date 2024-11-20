using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BT.Products.API.Data;
using BT.Products.API.Interface;
using BT.Shared.APIServiceLogs;
using BT.Shared.Domain;
using BT.Shared;
using BT.Shared.Interfaces;

namespace BT.Products.API.Repositories
{
    public class CategoryRepository(ProductDataContext context) : ICategory
    {
        public async Task<Response> CreateAsync(Category entity)
        {
            try
            {
                //  Validation if exist
                var categoryExist = await GetByAsync(_ => _.Id!.Equals(entity.Id));
                if (categoryExist is not null && !string.IsNullOrEmpty(categoryExist.Title))
                    return new Response(false, $"{entity.Title} already exist");

                var item = context.Category.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (item is not null && !string.IsNullOrEmpty(item.Id))
                {
                    return new Response(true, $"{entity.Title} added to databast at {DateTime.UtcNow.ToString()}");
                }
                else
                {
                    return new Response(false, $"{AppConstants.CreateDatabaseEntityFailed} {entity.Title} {DateTime.UtcNow.ToString()}");
                }
            }
            catch (Exception ex)
            {
                //  Log original exception
                LogException.LogExceptions(ex);

                // Display friendly message to client
                return new Response(false, AppConstants.CreateDatabaseEntityFailed + " " + DateTime.UtcNow.ToString());
            }
        }

        public async Task<Response> DeleteAsync(Category entity)
        {
            if (entity.Id is not null)
            {
                try
                {
                    var item = await FindByIdAsync(entity.Id);
                    if (item is null)
                    {
                        return new Response(false, $"{entity.Id} not found");
                    }

                    // Remove entity
                    context.Category.Remove(item);
                    await context.SaveChangesAsync();

                    return new Response(true, $"{AppConstants.DeleteDatabaseEntitySuccess} {entity.Title} {DateTime.UtcNow.ToString()}");
                }
                catch (Exception ex)
                {
                    //  Log original exception
                    LogException.LogExceptions(ex);

                    // Display friendly message to client
                    return new Response(false, AppConstants.DeleteDatabaseEntityFailed + " " + DateTime.UtcNow.ToString());
                }
            }
            else {
                return new Response(false, AppConstants.ParamsMissingError_Id + " " + DateTime.UtcNow.ToString());
            }
        }

        public async Task<Category> FindByIdAsync(string categoryCode)
        {
            try
            {
                var item = await context.Category.FindAsync(categoryCode);
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
                var items = await context.Category.AsNoTracking().ToListAsync();
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
                var items = await context.Category.Where(predicate).FirstOrDefaultAsync()!;
                return items is not null ? items : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception(AppConstants.ErrorRetrievingEntity);
            }

        }

        public async Task<Response> UdateAsync(Category entity)
        {

            if(entity.Id is not null)
            {
                try
                {
                    var item = await FindByIdAsync(entity.Id!);
                    if (item is null)
                        return new Response(false, $"{entity.Title} Not found");

                    context.Entry(item).State = EntityState.Deleted;
                    context.Category.Update(entity);
                    await context.SaveChangesAsync();
                    return new Response(true, $"{entity.Title} updated. {DateTime.UtcNow.ToString()}");

                }
                catch (Exception ex)
                {

                    LogException.LogExceptions(ex);
                    return new Response(false, AppConstants.UpdateDatabaseEntityFailed);

                }
            }
            else
            {
                return new Response(false, AppConstants.ParamsMissingError_Id);
            }
        }


        /// <summary>
        /// NOT USED
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Category> FindByIdAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT USED
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<Category> IGenericInterface<Category>.FindByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
