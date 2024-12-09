using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BT.Shared.APIServiceLogs;
using BT.Shared.Domain;
using BT.Shared;
using BT.Products.API.Data;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.Product;
using BT.Products.API.Domain;
using System.Linq;


namespace BT.Products.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        readonly ProductDataContext _context;
        readonly ILogger<ProductRepository> _logger;
        readonly IConfiguration _configuration;

        public ProductRepository(ProductDataContext dataContext, ILogger<ProductRepository> logger, IConfiguration configuration)
        {
            _context = dataContext;
            _logger = logger;
            _configuration = configuration;
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
                    return new APIResponseProduct(true, $"{entity.Title} added to databast at {DateTime.UtcNow.ToString()}", item.FromEntity());
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
                    .Include(s => s.TechSpecs)
                    .AsNoTracking()
                    .Select(p =>
                    new ProductDTO(
                        p.Id, 
                        p.Title,
                        p.Description,
                        p.Price,
                        p.CategoryId,
                        //add explict cast
                        ModelHelpers.FromEntity(p!.Images)!.ToList(),
                        p.StockQuantity,
                        ModelHelpers.ToProductSpecficationDTOCollection((p!.TechSpecs)!.ToList())
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

        public async Task<APIResponseProduct> UdateProductAsync(EditProductDTO dto)
        {
            try
            {
                var item = await GetProductFullAsync( dto.Id!.Value);
                if (item is null)
                    return new APIResponseProduct(false, $"Product with id: {dto.Id} was Not found");

                var updateProductSpecsSuccess = await UpdateProductTechSpecsAsync(item, dto);
                if (updateProductSpecsSuccess.Success)
                {
                    item = EditProduct(item, dto);
                    _context.Product.Entry(item).State = EntityState.Modified;
                    _context.Product.Update(item);
                    await _context.SaveChangesAsync();
                    // add the new spec
                    //itemToUpadate.Images = dto.Images.ToList();
                    return new APIResponseProduct(true, $"{dto.Title} updated. {DateTime.UtcNow.ToString()}", item.FromEntity());
                }
                else
                {
                    return new APIResponseProduct(false, $"{updateProductSpecsSuccess.message} {DateTime.UtcNow.ToString()}", item.FromEntity());
                }

            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);
                return new APIResponseProduct(false, AppConstants.UpdateDatabaseEntityFailed);

            }
        }


        async Task<bool> DeleteOldTechSpecsAsync(List<ProductSpecfication> deleteSpecs, Product product)
        {
            try
            {

                _context.ProductSpecfication.RemoveRange(deleteSpecs);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Sucessfully deleted old specs for product: {product.Id} .Timestamp : {DateTime.UtcNow}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete old specs for product: {product.Id} user's old roles. Exception: {ex.ToString()} Timestamp : {DateTime.UtcNow}");
                return false;
            }
        }

        async Task<bool> AddNewTechSpecsAsync(List<ProductSpecfication>? addSpecs, Product product)
        {
            try
            {
                if(addSpecs is not null) {
                    await _context.ProductSpecfication.AddRangeAsync(addSpecs);
                    await _context.SaveChangesAsync();
                }
                
                _logger.LogInformation($"Sucessfully added new specs for product: {product.Id} .Timestamp : {DateTime.UtcNow}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add new specs for product: {product.Id} user's old roles. Exception: {ex.ToString()} Timestamp : {DateTime.UtcNow}");
                return false;
            }
        }


        async Task<bool> DeleteAllTechSpecsAsync(int? productId)
        {
            if (!productId.HasValue)
                return false;

            try
            {
                var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == productId.Value);
                if(product is not null)
                {
                    var specs = await _context.ProductSpecfication
                    .Where(t => t.ProductId == product.Id)
                    .AsNoTracking()
                    .ToListAsync();

                    _context.ProductSpecfication.RemoveRange(specs);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Sucessfully added new specs for product: {product.Id} .Timestamp : {DateTime.UtcNow}");
                    return true;
                }
                else
                {
                    return false;
                }

                
            }
            catch(Exception Ex)
            {
                _logger.LogError($"Failed to delete product specs for product, with exception: {Ex.ToString()}. Timestamp : {DateTime.UtcNow}");
                return false;
            }
        }

        Product EditProduct(Product currentState, EditProductDTO updatedStated)
        {
            currentState.Title = updatedStated.Title;
            currentState.Description = updatedStated.Description;
            currentState.Price = updatedStated.Price;
            currentState.StockQuantity = updatedStated.StockQuantity;
            currentState.CategoryId = updatedStated.CategoryId;

            return currentState;
        }

        async Task<(bool Success, string? message)> UpdateProductTechSpecsAsync(Product currentState, EditProductDTO dto)
        {

            if (currentState.TechSpecs is not null && dto.TechSpecs is not null)
            {
                //  !currentState.TechSpecs.Any() && !dto.TechSpecs.Any()
                //  no update just return true
                if (!currentState.TechSpecs.Any() && !dto.TechSpecs.Any())
                {
                    return (true, string.Empty);
                }
                //  !currentState.Any() && dto.TechSpecs.Any
                //  add new collection
                else if(!currentState.TechSpecs.Any() && dto.TechSpecs.Any())
                {
                    var success = await AddNewTechSpecsAsync(ModelHelpers.FromEntity(dto.TechSpecs)!.ToList(), currentState);
                    if (success) {
                        return (true, string.Empty);
                    }
                    else
                    {
                        return (true, $"Unable to update product: {currentState.Title} with new TechSpecs.");
                    }
                }
                // currentState.Any() && dto.TechSpecs.Any()
                // its an edit
                else if(currentState.TechSpecs.Any() && dto.TechSpecs.Any())
                {

                    var newItems = dto.TechSpecs.Where(s => !currentState.TechSpecs.Any(x => x.Key == s.Key && x.Value == s.Value)).ToList();
                    var oldItems = currentState.TechSpecs.Where(s => !dto.TechSpecs.Any(x => x.Key == s.Key && x.Value == s.Value)).ToList();

                    bool operationSuccess = false;
                    string errorMsg="";

                    if (oldItems is not null)
                    {
                        operationSuccess = await DeleteOldTechSpecsAsync(oldItems, currentState);
                        if (!operationSuccess)
                        {
                            errorMsg = $"Unable to update Product: {currentState.Title} tech specs.";
                        }
                    }

                    if (newItems is not null)
                    {
                        operationSuccess = await AddNewTechSpecsAsync(ModelHelpers.ToEntity(newItems)!, currentState);
                        if (!operationSuccess)
                        {
                            errorMsg = $"Unable to update Product: {currentState.Title} tech specs.";
                        }
                    }

                    return (operationSuccess, errorMsg);
                }
                //  currentState.TechSpecs.Any() && !dto.TechSpecs.Any()
                //  delete all techspecs for this product
                else if(currentState.TechSpecs.Any() && !dto.TechSpecs.Any())
                {
                    var success = await DeleteAllTechSpecsAsync(currentState.Id!.Value);
                    if (success)
                    {
                        return (true, string.Empty);
                    }
                    else
                    {
                        return (false, $"Unable to update producd: {currentState.Title}");
                    }
                }
                //  catch all, just ignore
                else
                {
                    return (true, string.Empty);
                }
            }
            else {
                // My "just in case"  shoud never be null, but if this is the case
                // It's just a product property update, and hence we can ignore
                return (true, string.Empty);
            }


        }

        /// <summary>
        /// Helper function to get a <see cref="Product"/> that includes it
        /// Images and specifications
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        async Task<Product> GetProductFullAsync(int Id) {

            return await _context.Product
                .Include(i => i.Images)
                .Include(s => s.TechSpecs)
                .AsNoTracking()
                .FirstAsync(p => p.Id == Id);
        }

    }
}
