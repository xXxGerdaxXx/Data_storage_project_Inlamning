using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Services;

public class EmployeeService(IEmployeeRepository employeeRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<EmployeeDto?> RegisterEmployeeAsync(EmployeeRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Employee registration form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var existingEmployee = await _employeeRepository.GetAsync(e => e.Email == form.Email);
            if (existingEmployee != null)
                throw new ArgumentException("An employee with this email already exists.");

            var role = await _roleRepository.GetByIdAsync(form.RoleId);
            if (role == null)
                throw new KeyNotFoundException($"Role with ID {form.RoleId} not found.");

            var employee = new EmployeeEntity
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                RoleId = form.RoleId,
                Role = role
            };

            var createdEmployee = await _employeeRepository.CreateAsync(employee);
            return createdEmployee != null ? EmployeeFactory.Create(createdEmployee) : null;
        });
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllEmployeesWithRolesAsync();
        return employees.Select(EmployeeFactory.Create);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetEmployeeWithRoleAsync(employeeId);
        return employee != null ? EmployeeFactory.Create(employee) : null;
    }

    public async Task<EmployeeDto?> UpdateEmployeeAsync(EmployeeRegistrationForm form, int employeeId)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Employee form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var existingEmployee = await _employeeRepository.GetAsync(e => e.Id == employeeId)
                ?? throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            var role = await _roleRepository.GetByIdAsync(form.RoleId)
                ?? throw new KeyNotFoundException($"Role with ID {form.RoleId} not found.");

            existingEmployee.FirstName = form.FirstName;
            existingEmployee.LastName = form.LastName;
            existingEmployee.Email = form.Email;
            existingEmployee.RoleId = form.RoleId;
            existingEmployee.Role = role;

            var updatedEmployee = await _employeeRepository.UpdateAsync(existingEmployee, e => e.Id == employeeId);
            return updatedEmployee != null ? EmployeeFactory.Create(updatedEmployee) : null;
        });
    }

    public async Task<bool> DeleteEmployeeAsync(int employeeId)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            return await _employeeRepository.DeleteAsync(e => e.Id == employeeId);
        });
    }
}
