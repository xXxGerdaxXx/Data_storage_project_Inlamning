using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Factories;

public static class EmployeeRegistrationFactory
{
    public static EmployeeEntity CreateEmployee(EmployeeRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Employee registration form cannot be null.");

        if (string.IsNullOrWhiteSpace(form.FirstName) || string.IsNullOrWhiteSpace(form.LastName))
            throw new ArgumentException("First name and last name are required.");

        return new EmployeeEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            RoleId = form.RoleId
        };
    }
}
