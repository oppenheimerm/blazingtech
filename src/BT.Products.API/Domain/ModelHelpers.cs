using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Category;
using BT.Shared.Domain.DTO.Product;
using BT.Shared.Helpers;

namespace BT.Products.API.Domain
{
    public static class ModelHelpers
    {
        public static Category ToEntity(this CategoryDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new Category
                {
                    Id = dto.Id!.ToUpper(),
                    Title = dto.Title
                };
            }
        }

        public static CategoryDTO ToEntity(this Category dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new CategoryDTO
                {
                    Id = dto.Id,
                    Title = dto.Title
                };
            }
        }

        public static ProductImage ToEntity(this ProductImageDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new ProductImage
                {
                    Id = dto.Id,
                    ImageName = dto.ImageName,
                    ProductId = dto.ProductId
                };
            }
        }

        /// <summary>
        /// Returns an instance of a new <see cref="ProductImage"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ProductImage ToEntity(this CreatePhotoDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new ProductImage
                {
                    ImageName = dto.ImageName,
                    ProductId = dto.ProductId
                };
            }
        }


        public static ProductImageDTO ToEntity(this ProductImage dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new ProductImageDTO( dto.Id, dto.ImageName, dto.ProductId);
            }
        }

        public static Product ToEntity(this AddProductEntityDTO dto) {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new Product
                {
                    Title = StringHelpers.ToTitleCase(dto.Title!),
                    Description = dto.Description,
                    Price = dto.Price,
                    CategoryId = dto.CategoryId
                };
            }
        }

        public static Product ToEntity(this AddProductDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new Product
                {
                    Title = StringHelpers.ToTitleCase(dto.Title!),
                    Description = dto.Description,
                    Price = dto.Price,
                    CategoryId = dto.CategoryId
                };
            }
        }





        public static (CategoryDTO?, IEnumerable<CategoryDTO>?) FromEntity(Category category, IEnumerable<Category>? categories)
        {
            // Return single
            if (category is not null || categories is null)
            {
                var singleCategory = new CategoryDTO()
                {
                    Id = category!.Id,
                    Title = category.Title
                };

                return (singleCategory, null);
            }

            // Retun IEnumerable<T> list
            if (categories is not null || category is null)
            {
                var _categories = categories!.Select(c =>
                    new CategoryDTO()
                    {
                        Id = c!.Id,
                        Title = c.Title
                    }).ToList();

                return (null, _categories);
            }

            return (null, null);
        }

        public static IQueryable<ProductImageDTO>? FromEntity(ICollection<ProductImage>? images)
        {

            if (images is not null)
            {
                var _images = images!.Select( i =>
                    new ProductImageDTO(i!.Id, i.ImageName, i.ProductId));

                return _images.AsQueryable();
            }
            else
            {
                return Enumerable.Empty<ProductImageDTO>().AsQueryable();
            }

            
        }
    }
}
