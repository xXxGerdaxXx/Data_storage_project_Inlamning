using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IRoleService
{
    Task<RoleEntity?> RegisterRoleAsync(RoleRegistrationForm form);
    Task<IEnumerable<RoleEntity>> GetAllRolesAsync();
    Task<RoleEntity?> GetRoleByIdAsync(int roleId);
    Task<bool> DeleteRoleAsync(int roleId);
}
