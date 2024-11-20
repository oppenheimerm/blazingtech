using BT.Products.API.Domain;
using BT.Products.API.Interface;
using BT.Shared;
using Microsoft.AspNetCore.Mvc;
using BT.Shared.Domain.DTO.Product;

namespace BT.Products.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController(IProduct repository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await repository.GetAllAsync();
            if (!products.Any())
                return NotFound(AppConstants.DatabaseEntityNotFound);

            var (_, list) = ModelHelpers.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound(AppConstants.DatabaseEntityNotFound);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await repository.FindByIdAsync(id);
            if (product is null)
                return NotFound(AppConstants.DatabaseEntityNotFound);

            var (_product, _) = ModelHelpers.FromEntity(product, null!);
            return _product is not null ? Ok(_product) : NotFound(AppConstants.DatabaseEntityNotFound);
        }


        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEntity = ModelHelpers.ToEntity(product);
            var response = await repository.CreateAsync(newEntity);
            return response.flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEntity = ModelHelpers.ToEntity(product);
            var response = await repository.UdateAsync(newEntity);
            return response.flag is true ? Ok(response) : BadRequest(response);

        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            var entity = ModelHelpers.ToEntity(product);
            var response = await repository.DeleteAsync(entity);
            return response.flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
