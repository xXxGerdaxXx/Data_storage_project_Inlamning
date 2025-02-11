using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeEntity?> RegisterEmployeeAsync(EmployeeRegistrationForm form);
    Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync();
    Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId);
    Task<EmployeeEntity?> UpdateEmployeeAsync(EmployeeRegistrationForm form, int employeeId);
    Task<bool> DeleteEmployeeAsync(int employeeId);
}
