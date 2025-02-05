using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class EmployeeRepository(ApplicationDbContext context) : BaseRepository<EmployeeEntity>(context)
{
    private readonly ApplicationDbContext _context = context;

    // Override GetAllAsync to include Role when fetching employees
    public async Task<IEnumerable<EmployeeEntity>> GetAllEmployeesWithRolesAsync()
    {
        return await _context.Employees.Include(e => e.Role).ToListAsync();
    }

    // Override GetAsync to ensure single employee fetch also includes Role
    public async Task<EmployeeEntity?> GetEmployeeWithRoleAsync(int employeeId)
    {
        return await _context.Employees.Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
    }
}

