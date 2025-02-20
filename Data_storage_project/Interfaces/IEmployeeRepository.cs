using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IEmployeeRepository : IBaseRepository<EmployeeEntity>
{
    Task<IEnumerable<EmployeeEntity>> GetAllEmployeesWithRolesAsync();
    Task<EmployeeEntity?> GetEmployeeWithRoleAsync(int employeeId);
}
