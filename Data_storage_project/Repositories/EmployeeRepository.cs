using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class EmployeeRepository(ApplicationDbContext context, ILoggerService logger)
    : BaseRepository<EmployeeEntity>(context, logger), IEmployeeRepository
{
    public async Task<IEnumerable<EmployeeEntity>> GetAllEmployeesWithRolesAsync()
    {
        return await _context.Employees.Include(e => e.Role).ToListAsync();
    }

    public async Task<EmployeeEntity?> GetEmployeeWithRoleAsync(int employeeId)
    {
        return await _context.Employees.Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
    }
}
