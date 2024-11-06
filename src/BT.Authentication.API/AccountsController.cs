using BT.Authentication.API.DTO;
using BT.Authentication.API.Repositories;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BT.Authentication.API
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountsController : ControllerBase
    {
        readonly IAccount _iaccount;
        readonly IConfiguration _configuration;

        public AccountsController(IAccount iaccount, IConfiguration configuration)
        {
            _iaccount = iaccount;
            _configuration = configuration;
        }

        //[AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<BaseAPIResponseDTO>> RegisterAsync(RegisterDTO dto)
        {
            var result = await _iaccount.RegisterAsync(dto);
            return Ok(result);
        }

        //[AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<APIResponJWTDTO>> LoginAsync(LoginDTO dto)
        {
            var result = await _iaccount.LoginAsync(dto);
            return Ok(result);
        }

        [HttpPost("add-role")]
        public async Task<ActionResult<BaseAPIResponseDTO>> CreateRoleAsync(CreateRoleDTO dto)
        {
            var result = await _iaccount.CreateRoleAsync(dto);
            return Ok(result);
        }

        [HttpPost("add-user-to-role")]
        public async Task<ActionResult<BaseAPIResponseDTO>> AddUserToRoleAsync(AddUserToRoleDTO dto)
        {
            var result = await _iaccount.AddUserToRole(dto);
            return Ok(result);
        }

    }
}
