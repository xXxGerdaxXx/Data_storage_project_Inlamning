using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataStorageAPI.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            var role = await _roleService.GetRoleByIdAsync(roleId);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterRole([FromBody] RoleRegistrationForm form)
        {
            if (string.IsNullOrWhiteSpace(form.RoleName))
                return BadRequest("Role name is required.");

            var createdRole = await _roleService.RegisterRoleAsync(form);
            if (createdRole == null)
                return BadRequest("Role registration failed.");

            return CreatedAtAction(nameof(GetRoleById), new { roleId = createdRole.Id }, createdRole);
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] RoleRegistrationForm form)
        {
            if (string.IsNullOrWhiteSpace(form.RoleName))
                return BadRequest("Role name is required.");

            var updatedRole = await _roleService.UpdateRoleAsync(roleId, form);
            if (updatedRole == null)
                return NotFound();

            return Ok(updatedRole);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            var deleted = await _roleService.DeleteRoleAsync(roleId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
