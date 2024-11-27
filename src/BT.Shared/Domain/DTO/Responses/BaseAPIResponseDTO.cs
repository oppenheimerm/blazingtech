
using BT.Shared.Domain.DTO.Category;
using BT.Shared.Domain.DTO.Product;

namespace BT.Shared.Domain.DTO.Responses
{
    //  Where the uniary postfix operator "!" (null-forgiving operator)
    //  tells the compiler that passing null is expected and shouldn't be
    //  warned about.
    //  See: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving
    public record BaseAPIResponseDTO
    (
        bool Success = false,
        string Message = null!
    );

    public record APIResponJWTDTO(
        bool Success = false,
        string Message = null!,
        string JWTToken = null!
    ) : BaseAPIResponseDTO(Success, Message);

    public record APIResponseProduct(
        bool Success = false,
        string Message = null!,
        BT.Shared.Domain.Product? Product = null!
        ) :BaseAPIResponseDTO(Success, Message);

    public record APIResponseCategory(
        bool Success = false,
        string Message = null!,
        CategoryDTO? CategoryDTO = null!
        ) : BaseAPIResponseDTO(Success, Message);

    public record APIResponseProductImage(
        bool Success = false,
        string Message = null!,
        ProductImageDTO? ProductImageDTO = null!
        ) : BaseAPIResponseDTO(Success, Message);

}
