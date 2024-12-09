using BT.Products.API.Domain;
using BT.Shared;
using Microsoft.AspNetCore.Mvc;
using BT.Shared.Domain.DTO.Product;
using System.Text;
using BT.Products.API.Repositories;
using BT.Shared.Domain.DTO.Responses;
using System.Collections.Generic;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Category;

namespace BT.Products.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        IProductRepository _repository { get; set; }
        Random _random { get; }

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
            _random = new Random();
        }

        // includes images
        [HttpGet("all")]
        public ActionResult<IQueryable<ProductDTO>> GetProducts()
        {
            var products = _repository!.GetAllWithImages();
            return products!.Any() ? Ok(products) : Ok(new List<ProductDTO>());
        }

        //Authorize
        [HttpGet("new-image-filename")]
        public async Task<ActionResult<BaseAPIResponseDTO>> GenerateProductImageFileName(string ext, int numberOftries = 0)
        {
            var filename = GenerateImageFileName(12) + ext;
            var fileExist = await _repository!.ImageFileExistAsync(filename);
            if(fileExist == false)
            {
                return Ok(new BaseAPIResponseDTO() { Success = true, Message = filename});
            }
            else
            {
                if(numberOftries >= 14)
                {
                    return Ok(new BaseAPIResponseDTO() { Success = false, Message = "Unable to generate product" });
                }

                numberOftries += 1;
                return await GenerateProductImageFileName(ext, numberOftries);
            }


        }


        [HttpPost("create")]
        public async Task<ActionResult<APIResponseProduct>> CreateProduct(AddProductEntityDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(new APIResponseProduct()
                {
                    Message = "The form has errors, please check form and try again.",
                    Success = false,
                });

            var newEntity = ModelHelpers.ToEntity(product);
            var response = await _repository!.CreateAsync(newEntity);
            return response.Success is true ? Ok(response) : BadRequest(response);
        }

        
        [HttpPost("create-product-image")]
        public async Task<ActionResult<APIResponseProductImage>> CreateProductImage(CreatePhotoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new APIResponseProductImage()
                {
                    Message = "The form has errors, please check form and try again.",
                    Success = false,
                });

            var newEntity = ModelHelpers.ToEntity(dto);
            var response = await _repository!.CreatePhotoDbEntityAsync(newEntity);
            return response.Success is true ? Ok(response) : BadRequest(response);
        }


        [HttpPut("{Id}")]
        public async Task<ActionResult<APIResponseProduct>> UpdateCategory(int Id, EditProductDTO dto)
        {
            if (!ModelState.IsValid)
                return new APIResponseProduct() { Success = false, Message = "ModelState is invalid, please check form." };

            var response = await _repository.UdateProductAsync(dto);
            return Ok(response);
        }

        #region Helpers

        string GenerateImageFileName(int length)
        {
            var chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var output = new StringBuilder();
                      
            for (int i = 0; i < length; i++)
            {
                output.Append(chars[_random.Next(chars.Length)]);
            }

            return output.ToString();
        }

        #endregion
    }
}
