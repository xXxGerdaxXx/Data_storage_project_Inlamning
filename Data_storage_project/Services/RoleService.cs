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

    public async Task<RoleEntity?> RegisterRoleAsync(RoleRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Role form cannot be null.");

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var existingRole = await _roleRepository.GetAsync(r => r.RoleName == form.RoleName);
                if (existingRole != null)
                    throw new ArgumentException("Role name already exists.");

                var role = RoleRegistrationFactory.CreateRole(form);
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return role;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }
    }

    public async Task<IEnumerable<RoleEntity>> GetAllRolesAsync()
    {
        return await _roleRepository.GetAllAsync();
    }

    public async Task<RoleEntity?> GetRoleByIdAsync(int roleId)
    {
        return await _roleRepository.GetAsync(r => r.Id == roleId);
    }

    public async Task<RoleEntity?> UpdateRoleAsync(int roleId, RoleRegistrationForm form)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync()) // ✅ Added transaction
        {
            try
            {
                var existingRole = await _roleRepository.GetAsync(r => r.Id == roleId);
                if (existingRole == null)
                    return null;

                existingRole.RoleName = form.RoleName;

                await _context.SaveChangesAsync(); 
                await transaction.CommitAsync();
                return existingRole;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }
    }

    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync()) 
        {
            try
            {
                var role = await _roleRepository.GetAsync(r => r.Id == roleId);
                if (role == null)
                    throw new KeyNotFoundException($"Role with ID {roleId} not found.");

                _context.Roles.Remove(role); 
                await _context.SaveChangesAsync();

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
}
