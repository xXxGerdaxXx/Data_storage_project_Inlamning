using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto?> RegisterRoleAsync(RoleRegistrationForm form);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int roleId);
        Task<RoleDto?> UpdateRoleAsync(int roleId, RoleRegistrationForm form);
        Task<bool> DeleteRoleAsync(int roleId);
    }
}
