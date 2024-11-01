using BT.Shared.Domain;
using BT.Shared.Domain.DTO;

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
                    Id = dto.Id.ToUpper(),
                    Title = dto.Title
                };
            }
        }

        public static ProductAttribute ToEntity(this ProductAttributeDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new ProductAttribute { 
                    Id = dto.Id,
                    Key = dto.Key,
                    Value = dto.Value,
                    ProductId = dto.ProductId
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

        public static Product ToEntity(this ProductDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new Product
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Description = dto.Description,
                    Price = dto.Price,
                    CategoryId = dto.CategoryId,
                    Images = dto.Images,
                    Attributes = dto.Attributes                    
                };
            }
        }

        public static (CategoryDTO?, IEnumerable<CategoryDTO>?) FromEntity(Category category, IEnumerable<Category>? categories)
        {
            // Return single
            if (category is not null || categories is null)
            {
                var singleCategory = new CategoryDTO(
                        category!.Id!,
                        category!.Title!
                    );

                return (singleCategory, null);
            }

            // Retun IEnumerable<T> list
            if (categories is not null || category is null)
            {
                var _categories = categories!.Select(c =>
                    new CategoryDTO(c.Id!, c.Title!)).ToList();

                return (null, _categories);
            }

            return (null, null);
        }

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            // Return single
            if (product is not null || products is null)
            {
                var singleProduct = new ProductDTO(
                        product!.Id!,
                        product!.Title!,
                        product!.Description,
                        product.Price!.Value,
                        product!.CategoryId!,
                        product.Attributes,
                        product.Images
                    );

                return (singleProduct, null);
            }

            // Retun IEnumerable<T> list
            if (products is not null || product is null)
            {
                var _products = products!.Select(p =>
                    new ProductDTO(p.Id!, p.Title!, p.Description, p.Price!.Value, p.CategoryId!, p.Attributes!, p.Images! )).ToList();

                return (null, _products);
            }

            return (null, null);
        }
    }
}
