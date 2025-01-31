using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;

namespace Data_storage_project_library.Services;

public class EmployeeService(IBaseRepository<EmployeeEntity> employeeRepository) : IEmployeeService
{
    private readonly IBaseRepository<EmployeeEntity> _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));

    public async Task<EmployeeEntity?> RegisterEmployeeAsync(EmployeeRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Employee registration form cannot be null.");

        var existingEmployee = await _employeeRepository.GetAsync(e => e.Email == form.Email);
        if (existingEmployee != null)
            throw new ArgumentException("An employee with this email already exists.");

        var employee = EmployeeRegistrationFactory.CreateEmployee(form);

        return await _employeeRepository.CreateAsync(employee);
    }

    public async Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync()
    {
        return await _employeeRepository.GetAllAsync();
    }

    public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId)
    {
        return await _employeeRepository.GetAsync(e => e.Id == employeeId);
    }

    public async Task<EmployeeEntity?> UpdateEmployeeAsync(EmployeeRegistrationForm form, int employeeId)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Employee form cannot be null.");

        var existingEmployee = await _employeeRepository.GetAsync(e => e.Id == employeeId);
        if (existingEmployee == null)
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

        existingEmployee.FirstName = form.FirstName;
        existingEmployee.LastName = form.LastName;
        existingEmployee.Email = form.Email;
        existingEmployee.RoleId = form.RoleId;

        return await _employeeRepository.UpdateAsync(existingEmployee, e => e.Id == employeeId);
    }

    public async Task<bool> DeleteEmployeeAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetAsync(e => e.Id == employeeId);
        if (employee == null)
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

        return await _employeeRepository.DeleteAsync(e => e.Id == employeeId);
    }
}
