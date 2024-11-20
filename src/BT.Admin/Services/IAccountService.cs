using BT.Shared.Domain.DTO;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.User;
using Microsoft.AspNetCore.Components;

namespace BT.Admin.Services
{
    /// <summary>
    /// Account managment service for front end clients
    /// </summary>
    public interface IAccountService
    {
        Task<BaseAPIResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<APIResponJWTDTO> LoginAsync(LoginDTO dto);
        Task GetRefreshToken(NavigationManager navigationManager);
        Task<WeatherForcastDTO[]?> GetWeatherForcast(NavigationManager navigationManager);
    }
}
