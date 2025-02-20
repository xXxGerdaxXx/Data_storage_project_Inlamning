using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;

namespace Data_storage_project_library.Services;

public class RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork) : IRoleService
{
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RoleDto?> RegisterRoleAsync(RoleRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Role form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var existingRole = await _roleRepository.GetByRoleNameAsync(form.RoleName);
            if (existingRole != null)
                throw new ArgumentException("Role name already exists.");

            var role = new RoleEntity { RoleName = form.RoleName };
            var createdRole = await _roleRepository.CreateAsync(role);

            return createdRole != null ? RoleFactory.Create(createdRole) : null;
        });
    }


    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles.Select(RoleFactory.Create).ToList(); 
    }

    public async Task<RoleDto?> GetRoleByIdAsync(int roleId)
    {
        var role = await _roleRepository.GetAsync(r => r.Id == roleId);
        return role != null ? RoleFactory.Create(role) : null;
    }

    public async Task<RoleDto?> UpdateRoleAsync(int roleId, RoleRegistrationForm form)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var existingRole = await _roleRepository.GetAsync(r => r.Id == roleId);
            if (existingRole == null)
                return null;

            existingRole.RoleName = form.RoleName;
            var updatedRole = await _roleRepository.UpdateAsync(existingRole, r => r.Id == roleId);

            return updatedRole != null ? RoleFactory.Create(updatedRole) : null;
        });
    }

    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var role = await _roleRepository.GetAsync(r => r.Id == roleId);
            if (role == null)
                return false;

            return await _roleRepository.DeleteAsync(r => r.Id == roleId);
        });
    }
}
