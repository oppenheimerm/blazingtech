using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace BT.Shared.Domain.DTO.Product
{
    public class AddProductDTO
    {
        [Required]
        [MaxLength(200, ErrorMessage = "The title field has a max length of 200 characters.")]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = "The description field has a max length of 1000 characters.")]
        public string? Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        public int? StockQuantity { get; set; }

        [Required]
        public string? CategoryId { get; set; }

        public IBrowserFile? Image { get; set; }
    }

    /// <summary>
    /// And instance of a <see cref="AddProductDTO"/> with out the Image(<see cref="IBrowserFile"/>)
    /// </summary>
    public class AddProductEntityDTO
    {
        [Required]
        [MaxLength(200, ErrorMessage = "The title field has a max length of 200 characters.")]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = "The description field has a max length of 1000 characters.")]
        public string? Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        public int? StockQuantity { get; set; }

        [Required]
        public string? CategoryId { get; set; }

    }


}
