using Microsoft.AspNetCore.Mvc;
using BT.Shared;
using BT.Shared.Domain.DTO;
using BT.Products.API.Domain;
using BT.Products.API.Interface;


namespace BT.Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategory repository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await repository.GetAllAsync();
            if (!categories.Any())
                return NotFound("No categories found.");

            var (_, list) = ModelHelpers.FromEntity(null!, categories);
            return list!.Any() ? Ok(list) : NotFound("No categories found.");
        }

        [Route("CategoryCode")]
        [HttpGet]
        public async Task<ActionResult<CategoryDTO>> GetProduct(string catCode)
        {
            var category = await repository.FindByIdAsync(catCode);
            if (category is null)
                return NotFound(AppConstants.DatabaseEntityNotFound);

            var (_category, _) = ModelHelpers.FromEntity(category, null!);
            return _category is not null ? Ok(_category) : NotFound(AppConstants.DatabaseEntityNotFound);
        }

        [HttpPost]
        public async Task<ActionResult<Response>>CreateCategory(CategoryDTO category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEntity = ModelHelpers.ToEntity(category);
            var response = await repository.CreateAsync(newEntity);
            return response.flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateCategory(CategoryDTO category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEntity = ModelHelpers.ToEntity(category);
            var response = await repository.UdateAsync(newEntity);
            return response.flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProduct(CategoryDTO category)
        {
            var entity = ModelHelpers.ToEntity(category);
            var response = await repository.DeleteAsync(entity);
            return response.flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
