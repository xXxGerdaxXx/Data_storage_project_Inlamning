using Data_storage_project_library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Interfaces;

public interface IRoleRepository : IBaseRepository<RoleEntity>
{
    Task<RoleEntity?> GetByRoleNameAsync(string roleName);
    Task<RoleEntity?> GetByIdAsync(int roleId);  
}
