using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BT.Shared.APIServiceLogs;
using BT.Shared.Domain;
using BT.Shared;
using BT.Products.API.Data;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.Product;
using BT.Products.API.Domain;

namespace BT.Products.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        readonly ProductDataContext _context;

        public ProductRepository(ProductDataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<APIResponseProduct> CreateAsync(Product entity)
        {
            try
            {
                //  Validation if exist
                var productExist = await GetByAsync(_ => _.Title!.Equals(entity.Title));
                if (productExist is not null && !string.IsNullOrEmpty(productExist.Title))
                    return new APIResponseProduct(false, $"{entity.Title} already exist", null);

                var item = _context.Product.Add(entity).Entity;
                await _context.SaveChangesAsync();
                if (item is not null && item.Id > 0)
                {
                    return new APIResponseProduct(true, $"{entity.Title} added to databast at {DateTime.UtcNow.ToString()}", item);
                }
                else
                {
                    return new APIResponseProduct(false, $"{AppConstants.CreateDatabaseEntityFailed} {entity.Title} {DateTime.UtcNow.ToString()}", null);
                }
            }
            catch (Exception ex)
            {
                //  Log original exception
                LogException.LogExceptions(ex);

                // Display friendly message to client
                return new APIResponseProduct(false, AppConstants.CreateDatabaseEntityFailed + " " + DateTime.UtcNow.ToString(), null);
            }
        }

        public async Task<APIResponseProductImage> CreatePhotoDbEntityAsync(ProductImage entity)
        {
            try
            {
                var item = _context.ProductImage.Add(entity).Entity;
                await _context.SaveChangesAsync();
                if (item is not null && item.Id > 0)
                {
                    return new APIResponseProductImage(true, $"{entity.ImageName} added to databast at {DateTime.UtcNow.ToString()}", item.ToEntity());
                }
                else
                {
                    return new APIResponseProductImage(false, $"{AppConstants.CreateDatabaseEntityFailed} {entity.ImageName} {DateTime.UtcNow.ToString()}", null);
                }
            }
            catch (Exception ex)
            {
                //  Log original exception
                LogException.LogExceptions(ex);

                // Display friendly message to client
                return new APIResponseProductImage(false, AppConstants.CreateDatabaseEntityFailed + " " + DateTime.UtcNow.ToString(), null);
            }
        }

        public async Task<BaseAPIResponseDTO> DeleteAsync(Product entity)
        {
            try
            {
                var item = await FindByIdAsync(entity.Id!.Value);
                if (item is null)
                {
                    return new BaseAPIResponseDTO(false, $"{entity.Id} not found");
                }

                // Remove entity
                _context.Product.Remove(item);
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

        public async Task<Product> FindByIdAsync(int Id)
        {
            try
            {
                var item = await _context.Product
                    .Where( p => p.Id == Id)
                    .FirstOrDefaultAsync();

                return item is not null ? item : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception(AppConstants.ErrorRetrievingEntity);
            }
        }

        public async Task<bool> ImageFileExistAsync(string imageName)
        {
            var item = await _context.ProductImage
                .Where(f => f.ImageName == imageName)
                .FirstOrDefaultAsync();

            return (item is null) ? false : true;
        }


        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                //  Remember ToList() iterates, this must be changed
                var items = await _context.Product
                    .AsNoTracking()
                    .ToListAsync();
                return items is not null ? items : Enumerable.Empty<Product>();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception(AppConstants.ErrorRetrievingEntity);
            }
        }

        public IQueryable<ProductDTO>? GetAllWithImages()
        {
            try
            {
                //  Remember ToList() iterates, this must be changed
                var items = _context.Product
                    .Include(i => i.Images)
                    .AsNoTracking()
                    .Select(p =>
                    new ProductDTO(
                        p.Id, 
                        p.Title,
                        p.Description,
                        p.Price,
                        p.CategoryId,
                        //add explict cast
                        ModelHelpers.FromEntity(p!.Images)!.ToList()
                        )
                    );

                   
                return items;
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
                var items = await _context.Product.Where(predicate).FirstOrDefaultAsync()!;
                return items is not null ? items : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception(AppConstants.ErrorRetrievingEntity);
            }

        }

        public async Task<BaseAPIResponseDTO> UdateAsync(Product entity)
        {
            try
            {
                var item = await FindByIdAsync(entity.Id!.Value);
                if (item is null)
                    return new BaseAPIResponseDTO(false, $"{entity.Title} Not found");

                _context.Product.Entry(item).State = EntityState.Modified;
                _context.Product.Update(entity);
                await _context.SaveChangesAsync();
                return new BaseAPIResponseDTO(true, $"{entity.Title} updated. {DateTime.UtcNow.ToString()}");

            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);
                return new BaseAPIResponseDTO(false, AppConstants.UpdateDatabaseEntityFailed);

            }
        }


    }
}
