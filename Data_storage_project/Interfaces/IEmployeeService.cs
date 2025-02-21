using Data_storage_project_library.Dtos;


namespace Data_storage_project_library.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto?> RegisterEmployeeAsync(EmployeeRegistrationForm form);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(int employeeId); 
    Task<EmployeeDto?> UpdateEmployeeAsync(EmployeeRegistrationForm form, int employeeId); 
    Task<bool> DeleteEmployeeAsync(int employeeId);

    Task<bool> ExistsAsync(int employeeId);  
}
