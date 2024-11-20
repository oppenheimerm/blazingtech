using BT.Shared.APIServiceLogs;
using BT.Shared.Domain;
using BT.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BT.Products.API.Interface;
using BT.Products.API.Data;

namespace BT.Products.API.Repositories
{
    public class ProductRepository(ProductDataContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                //  Validation if exist
                var productExist = await GetByAsync(_ => _.Title!.Equals(entity.Title));
                if (productExist is not null && !string.IsNullOrEmpty(productExist.Title))
                    return new Response(false, $"{entity.Title} already exist");

                var item = context.Product.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (item is not null && item.Id > 0)
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

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var item = await FindByIdAsync(entity.Id!.Value);
                if (item is null)
                {
                    return new Response(false, $"{entity.Id} not found");
                }

                // Remove entity
                context.Product.Remove(item);
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

        public async Task<Product> FindByIdAsync(int Id)
        {
            try
            {
                var item = await context.Product.FindAsync(Id);
                return item is not null ? item : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception(AppConstants.ErrorRetrievingEntity);
            }
        }

        /// <summary>
        /// Not Used as id's are ints   
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Product> FindByIdAsync(Guid Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// NOT USED
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Product> FindByIdAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var items = await context.Product.AsNoTracking().ToListAsync();
                return items is not null ? items : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception(AppConstants.ErrorRetrievingEntity);
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var items = await context.Product.Where(predicate).FirstOrDefaultAsync()!;
                return items is not null ? items : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception(AppConstants.ErrorRetrievingEntity);
            }

        }

        public async Task<Response> UdateAsync(Product entity)
        {
            try
            {
                var item = await FindByIdAsync(entity.Id!.Value);
                if (item is null)
                    return new Response(false, $"{entity.Title} Not found");

                context.Entry(item).State = EntityState.Modified;
                context.Product.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Title} updated. {DateTime.UtcNow.ToString()}");

            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);
                return new Response(false, AppConstants.UpdateDatabaseEntityFailed);

            }
        }
    }
}
