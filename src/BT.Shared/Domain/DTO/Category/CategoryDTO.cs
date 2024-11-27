
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BT.Shared.Domain.DTO.Category
{
    public class CategoryDTO
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(4, ErrorMessage = "Category id must be 4 characters long"), MinLength(4)]
        public string? Id { get; set; }

        [StringLength(50)]
        [Required]
        public string? Title { get; set; }
    }
}
