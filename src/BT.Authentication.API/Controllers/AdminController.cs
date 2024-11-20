using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BT.Authentication.API.Repositories;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.User;

using System.Net.Http;

namespace BT.Authentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "MUSR")]

    public class AdminController : ControllerBase
    {
        readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }


        //[AllowAnonymous]
        [HttpPost("create-user")]
        public async Task<ActionResult<BaseAPIResponseDTO>> CreateUserAsync(NewUserDTO dto)
        {
            var result = await _adminRepository.CreateUserAsync(dto);
            return Ok(result);
        }

        [HttpPost("add-role")]
        public async Task<ActionResult<BaseAPIResponseDTO>> CreateRoleAsync(AddRoleDTO dto)
        {
            var result = await _adminRepository.AddRoleAsync(dto);
            return Ok(result);
        }


        [HttpPost("add-user-to-role")]
        public async Task<ActionResult<BaseAPIResponseDTO>> AddUserToRoleAsync(AddUserToRoleDTO dto)
        {
            var result = await _adminRepository.AddUserToRole(dto);
            return Ok(result);
        }


        //  TODO Leaking Role structure, change to RoleLiteDTO remove User information(BTUSer) dependency
        [HttpGet("roles")]
        //[Authorize(Roles = "EMPLOYEE_IT")]
        public async Task<ActionResult<List<RoleLiteDTO>>> GetRoles()
        {
            /*var result = await _adminRepository.GetRoles();
            return Ok(result);*/

            var result = await _adminRepository.GetRoles();
            return Ok(result);
        }

        [HttpGet("user-roles")]
        public async Task<ActionResult<List<RoleLiteDTO>>> GetUserRoles( Guid Id)
        {
            var results = await _adminRepository.GetUserRolesAsync(Id);
            return Ok(results);
        }

        [HttpGet("users")]
        public ActionResult<IQueryable<BTUserLite>> GetUsers()
        {
            var result = _adminRepository.GetUsers();
            return Ok(result);
        }

        [HttpPut("update-user")]
        public async Task<ActionResult<BaseAPIResponseDTO>> UpDateUser(Guid userId, EditUserDTO dto)
        {
            var result = await _adminRepository.UpdateUserAsync(dto);
            return Ok(result);
        }

    }
}
