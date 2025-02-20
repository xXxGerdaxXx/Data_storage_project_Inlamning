using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class RoleRepository(ApplicationDbContext context, ILoggerService logger)
    : BaseRepository<RoleEntity>(context, logger), IRoleRepository
{
    public async Task<RoleEntity?> GetByRoleNameAsync(string roleName)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.RoleName == roleName);
    }

    public async Task<RoleEntity?> GetByIdAsync(int roleId) 
    {
        return await _dbSet.FindAsync(roleId);
    }
}
