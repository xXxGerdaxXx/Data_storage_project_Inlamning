using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;

namespace Data_storage_project_library.Services;

public class RoleService(IBaseRepository<RoleEntity> roleRepository) : IRoleService
{
    private readonly IBaseRepository<RoleEntity> _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));

    public async Task<RoleEntity?> RegisterRoleAsync(RoleRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Role form cannot be null.");

        // Check if the role already exists
        var existingRole = await _roleRepository.GetAsync(r => r.RoleName == form.RoleName);
        if (existingRole != null)
            throw new ArgumentException("Role name already exists.");

        // Create RoleEntity using the factory
        var role = RoleRegistrationFactory.CreateRole(form);

        // Save to the database
        return await _roleRepository.CreateAsync(role);
    }

    public async Task<IEnumerable<RoleEntity>> GetAllRolesAsync()
    {
        return await _roleRepository.GetAllAsync();
    }

    public async Task<RoleEntity?> GetRoleByIdAsync(int roleId)
    {
        return await _roleRepository.GetAsync(r => r.Id == roleId);
    }

    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        return await _roleRepository.DeleteAsync(r => r.Id == roleId);
    }
}
