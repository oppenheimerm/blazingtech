
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BT.Shared.Domain
{
    public class ProductSpecfication
    {
        [Key]
        public int? Id { get; set; }
        /// <summary>
        /// Spec type
        /// </summary>
        [Required]
        [MaxLength(30, ErrorMessage = "The Key field has a max length of 30 characters.")]
        public string? Key { get; set; }

        /// <summary>
        /// Value for this specification
        /// </summary>
        [Required]
        [MaxLength(30, ErrorMessage = "The Value field has a max length of 0 characters.")]
        public string? Value { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public int? ProductId { get; set; }

        public Product? Product { get; set; }

        public override bool Equals(Object? obj)
        {
            if (obj == null)
                return false;
            var spec = (ProductSpecfication)obj;
            return spec.Key == Key && spec.Value == Value && spec.ProductId == ProductId;
        }

        /// <summary>
        /// For has base comparison
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { Id, Key, Value, ProductId }.GetHashCode();
    }
}
