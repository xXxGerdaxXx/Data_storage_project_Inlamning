using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Services;

public class EmployeeService(IBaseRepository<EmployeeEntity> _employeeRepository, ApplicationDbContext _context) : IEmployeeService
{
    public async Task<EmployeeDto?> RegisterEmployeeAsync(EmployeeRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Employee registration form cannot be null.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingEmployee = await _employeeRepository.GetAsync(e => e.Email == form.Email);
            if (existingEmployee != null)
                throw new ArgumentException("An employee with this email already exists.");

            var role = await _context.Roles.FindAsync(form.RoleId);
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

            await _employeeRepository.CreateAsync(employee);
            await transaction.CommitAsync();

            return EmployeeFactory.Create(employee); 
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _context.Employees
            .Include(e => e.Role) 
            .ToListAsync();

        return employees.Select(EmployeeFactory.Create).ToList();
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int employeeId)
    {
        var employee = await _context.Employees
            .Include(e => e.Role) 
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        return employee != null ? EmployeeFactory.Create(employee) : null;
    }

    public async Task<EmployeeDto?> UpdateEmployeeAsync( EmployeeRegistrationForm form, int employeeId)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Employee form cannot be null.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingEmployee = await _employeeRepository.GetAsync(e => e.Id == employeeId);
            if (existingEmployee == null)
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

            var role = await _context.Roles.FindAsync(form.RoleId);
            if (role == null)
                throw new KeyNotFoundException($"Role with ID {form.RoleId} not found.");

            existingEmployee.FirstName = form.FirstName;
            existingEmployee.LastName = form.LastName;
            existingEmployee.Email = form.Email;
            existingEmployee.RoleId = form.RoleId;
            existingEmployee.Role = role;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return EmployeeFactory.Create(existingEmployee); // ✅ Convert to DTO
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteEmployeeAsync(int employeeId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var employee = await _employeeRepository.GetAsync(e => e.Id == employeeId);
            if (employee == null)
                return false;

            await _employeeRepository.DeleteAsync(e => e.Id == employeeId);
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