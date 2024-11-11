namespace BT.Shared.Domain.DTO.Responses
{
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

}
