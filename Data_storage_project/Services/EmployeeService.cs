using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Services;

public class EmployeeService(EmployeeRepository employeeRepository, ApplicationDbContext context) : IEmployeeService
{
    private readonly EmployeeRepository _employeeRepository = employeeRepository;
    private readonly ApplicationDbContext _context = context;

    public async Task<EmployeeEntity?> RegisterEmployeeAsync(EmployeeRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Employee registration form cannot be null.");

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetAsync(e => e.Email == form.Email);
                if (existingEmployee != null)
                    throw new ArgumentException("An employee with this email already exists.");
                var roleExist = await _context.Roles.AnyAsync(r => r.Id == form.RoleId);
                if (!roleExist)
                    throw new KeyNotFoundException($"Role with ID {form.RoleId} not found.");

                var employee = EmployeeRegistrationFactory.CreateEmployee(form);

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return employee;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
}

    public async Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync()
    {
        return await _employeeRepository.GetAllAsync(e => e.Role); // Eagerly load Role
    }


    public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId)
    {
        return await _employeeRepository.GetAsync(e => e.Id == employeeId, e => e.Role); // Eagerly load Role
    }


    public async Task<EmployeeEntity?> UpdateEmployeeAsync(EmployeeRegistrationForm form, int employeeId)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Employee form cannot be null.");

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetAsync(e => e.Id == employeeId);
                if (existingEmployee == null)
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

                var roleExist = await _context.Roles.AnyAsync(r => r.Id == form.RoleId);
                if (!roleExist)
                    throw new KeyNotFoundException($"Role with ID {form.RoleId} not found.");

                existingEmployee.FirstName = form.FirstName;
                existingEmployee.LastName = form.LastName;
                existingEmployee.Email = form.Email;
                existingEmployee.RoleId = form.RoleId;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return existingEmployee;

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }


        }
    }

    public async Task<bool> DeleteEmployeeAsync(int employeeId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var employee = await _employeeRepository.GetAsync(e => e.Id == employeeId);
                if (employee == null)
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

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


}
