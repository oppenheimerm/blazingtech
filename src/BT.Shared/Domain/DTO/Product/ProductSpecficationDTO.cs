
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO.Product
{
    public class ProductSpecficationDTO
    {
        /// <summary>
        /// Spec type
        /// </summary>
        [Required]
        [MaxLength(30, ErrorMessage = "Product specfication key has a max length of 30 characters.")]
        public string? Key { get; set; }

        /// <summary>
        /// Value for this specification
        /// </summary>
        [Required]
        [MaxLength(30, ErrorMessage = "Product specfication value has a max length of 30 characters.")]
        public string? Value { get; set; }

        [Required]
        public int? ProductId { get; set; }
    }

}
