using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;

namespace Data_storage_project_library.Services;

public class RoleService(RoleRepository roleRepository, ApplicationDbContext context) : IRoleService
{
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly ApplicationDbContext _context = context;

    public async Task<RoleDto?> RegisterRoleAsync(RoleRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Role form cannot be null.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingRole = await _roleRepository.GetAsync(r => r.RoleName == form.RoleName);
            if (existingRole != null)
                throw new ArgumentException("Role name already exists.");

            var role = new RoleEntity
            {
                RoleName = form.RoleName
            };

            var createdRole = await _roleRepository.CreateAsync(role);

            if (createdRole == null)  
            {
                await transaction.RollbackAsync();
                return null;
            }

            await transaction.CommitAsync();
            return RoleFactory.Create(createdRole); 
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
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
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingRole = await _roleRepository.GetAsync(r => r.Id == roleId);
            if (existingRole == null)
                return null;

            existingRole.RoleName = form.RoleName;

            var updatedRole = await _roleRepository.UpdateAsync(existingRole, r => r.Id == roleId);

            if (updatedRole == null) 
            {
                await transaction.RollbackAsync();
                return null;
            }

            await transaction.CommitAsync();
            return RoleFactory.Create(updatedRole);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var role = await _roleRepository.GetAsync(r => r.Id == roleId);
            if (role == null)
                return false;

            await _roleRepository.DeleteAsync(r => r.Id == roleId);
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
