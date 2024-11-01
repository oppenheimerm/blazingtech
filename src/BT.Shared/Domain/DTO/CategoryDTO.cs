
using System.ComponentModel.DataAnnotations;

namespace BT.Shared.Domain.DTO
{
    /// <summary>
    /// Using the simplicity of C# records
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Title"></param>
    public record CategoryDTO
    (
        [Required, MaxLength(4, ErrorMessage = "Category requires a code of 4 characters long"), MinLength(4)] string Id,
        [Required, StringLength(50)] string Title
    );
}
