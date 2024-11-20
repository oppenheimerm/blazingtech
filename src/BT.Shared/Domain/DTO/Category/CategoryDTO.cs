
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BT.Shared.Domain.DTO.Category
{
    /// <summary>
    /// Using the simplicity of C# records
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Title"></param>
    /*public record CategoryDTO
    (
        [Required, MaxLength(4, ErrorMessage = "Category requires a code of 4 characters long"), MinLength(4)] string Id,
        [Required, StringLength(50)] string Title
    );*/

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
