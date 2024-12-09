using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BT.Shared.Domain
{
    public class Product
    {


        /// <summary>
        /// In some cases, equality is tested explicitly (direct comparison) and implicitly (in operations like union, except, 
        /// intersect etc) in other cases. When it comes to the indirect or implicit test for equality, is where we need to take 
        /// extra care of object comparison.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object? obj)
        {
            if (obj == null)
                return false;
            var prod = (Product)obj;
            return prod.Title == Title && prod.CategoryId == CategoryId;
        }

        /// <summary>
        /// For has base comparison
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { Id, CategoryId }.GetHashCode();

        [Key]
        public int? Id { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "The title field has a max length of 200 characters.")]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = "The description field has a max length of 1000 characters.")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        public int? StockQuantity { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public string? CategoryId { get; set; }

        public Category? Category { get; set; }

        public string GetFormattedPrice() => Price!.Value.ToString("0.00");

        public ICollection<ProductImage>? Images { get; set; }
        public ICollection<ProductSpecfication>? TechSpecs { get; set; }
    }
}
