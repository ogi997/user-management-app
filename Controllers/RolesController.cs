using API.DTOs.RoleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Services;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("/api/[controller]")]
    public class RolesController(IRolesService roleService) : ControllerBase
    {
        private readonly IRolesService _roleService = roleService;

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO createRoleDTO)
        {
            var result = await _roleService.CreateRoleAsync(createRoleDTO);

            if (result == "Role created successfully")
            {
                return Ok(new { Message = result });
            }

            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponseDTO>>> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleService.DeleteRoleAsync(id);

            if (result == "Role deleted successfully.")
            {
                return Ok(new { Message = result });
            }

            return BadRequest(result);
        }

        [HttpPost("Assign")]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignDTO roleAssignDTO)
        {
            var result = await _roleService.AssignRoleAsync(roleAssignDTO);

            if (result == "Role assigned successfully.")
            {
                return Ok(new { Message = result });
            }

            return BadRequest(result);
        }
    }
}
