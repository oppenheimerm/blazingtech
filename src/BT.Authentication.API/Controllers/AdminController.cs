using BT.Authentication.API.Repositories;
using BT.Shared.Domain;
using BT.Shared.Domain.DTO;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace BT.Authentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class AdminController : ControllerBase
    {
        readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
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


        [HttpGet("roles")]
        //[Authorize(Roles = "EMPLOYEE_IT")]
        public async Task<ActionResult<List<Role>>> GetRoles()
        {
            var result = await _adminRepository.GetRoles();
            return Ok(result);
        }

    }
}
