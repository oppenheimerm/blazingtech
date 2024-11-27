using Microsoft.AspNetCore.Mvc;
using BT.Shared;
using BT.Products.API.Domain;
using BT.Shared.Domain.DTO.Category;
using BT.Products.API.Repositories;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain;


namespace BT.Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ICategoryRepository _repository { get; set; }

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _repository.GetAllAsync();            
            return categories!.Any() ? Ok(categories) : Ok(Enumerable.Empty<CategoryDTO>());
        }


        [Route("CategoryCode")]
        [HttpGet]
        public async Task<ActionResult<CategoryDTO>> GetProduct(string catCode)
        {
            var category = await _repository.FindByIdAsync(catCode);
            if (category is null)
                return NotFound(AppConstants.DatabaseEntityNotFound);

            var (_category, _) = ModelHelpers.FromEntity(category, null!);
            return _category is not null ? Ok(_category) : NotFound(AppConstants.DatabaseEntityNotFound);
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponseCategory>>CreateCategory(CategoryDTO category)
        {
            if (!ModelState.IsValid)
                return new APIResponseCategory() { 
                    CategoryDTO = null,
                    Message = "",
                    Success = false
                };

            var newEntity = ModelHelpers.ToEntity(category);
            var response = await _repository.CreateAsync(newEntity);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<BaseAPIResponseDTO>> UpdateCategory(CategoryDTO category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEntity = ModelHelpers.ToEntity(category);
            var response = await _repository.UdateAsync(newEntity);
            return response.Success is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<BaseAPIResponseDTO>> DeleteProduct(CategoryDTO category)
        {
            var entity = ModelHelpers.ToEntity(category);
            var response = await _repository.DeleteAsync(entity);
            return response.Success is true ? Ok(response) : BadRequest(response);
        }
    }
}
