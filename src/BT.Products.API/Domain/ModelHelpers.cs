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
                    CategoryId = dto.CategoryId,
                    StockQuantity = dto.StockQuantity
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


        /// <summary>
        /// The collection of <see cref="ProductSpecfication"/> does not contain Id's
        /// </summary>
        /// <param name="dtoCollection"></param>
        /// <returns></returns>
        public static IEnumerable<ProductSpecfication>? FromEntity(IEnumerable<ProductSpecficationDTO>? dtoCollection)
        {

            if(dtoCollection is not null)
            {
                var _specCollection = dtoCollection.Select(i => new ProductSpecfication()
                {
                    Key = i.Key,
                    Value = i.Value,
                    ProductId = i.ProductId
                });

                return _specCollection;
            }
            else
            {
                return null!;
            }
            
        }

        public static ProductDTO FromEntity(this Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            else
            {
                return new ProductDTO(
                    product.Id,
                    product.Title,
                    product.Description,
                    product.Price,
                    product.CategoryId,
                    product.Images is null ? null : product.Images.Select(_ => new ProductImageDTO(_.Id, _.ImageName, _.ProductId)).ToList(),
                    product.StockQuantity,
                    product.TechSpecs is null ? null : product.TechSpecs.Select(_ => new ProductSpecficationDTO()
                    {
                        ProductId = _.ProductId,
                        Key = _.Key,
                        Value = _.Value
                    }).ToList());
            };
        }


        /// <summary>
        /// Converts a ICollection(ProductSpecification) to List(ProductSpecificationDTO)
        /// </summary>
        /// <param name="dtoCollection"></param>
        /// <returns></returns>

        public static List<ProductSpecficationDTO>? ToProductSpecficationDTOCollection(ICollection<ProductSpecfication>? dtoCollection)
        {

            if (dtoCollection is not null)
            {
                var _specCollection = dtoCollection.Select(i => new ProductSpecficationDTO()
                {
                    Key = i.Key,
                    Value = i.Value,
                    ProductId = i.ProductId
                });

                return _specCollection.ToList();
            }
            else
            {
                return null!;
            }

        }

        public static List<ProductSpecfication>? ToEntity(List<ProductSpecficationDTO>? dtoCollection)
        {

            if (dtoCollection is not null)
            {
                var _specCollection = dtoCollection.Select(i => new ProductSpecfication()
                {
                    Key = i.Key,
                    Value = i.Value,
                    ProductId = i.ProductId
                });

                return _specCollection.ToList();
            }
            else
            {
                return null!;
            }

        }
    }

}
