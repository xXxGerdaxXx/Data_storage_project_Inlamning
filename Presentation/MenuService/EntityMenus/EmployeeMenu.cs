using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Entities;

namespace Presentation.MenuService.EntityMenus;

public class EmployeeMenu(IEmployeeService employeeService)
{
    private readonly IEmployeeService _employeeService = employeeService;

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Employee Management ===");
            Console.WriteLine("1. View All Employees");
            Console.WriteLine("2. Add New Employee");
            Console.WriteLine("3. Update Employee");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ViewAllEmployeesAsync();
                    break;
                case "2":
                    await AddNewEmployeeAsync();
                    break;
                case "3":
                    await UpdateEmployeeAsync();
                    break;
                case "4":
                    await DeleteEmployeeAsync();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ViewAllEmployeesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Employees ===");

        var employees = await _employeeService.GetAllEmployeesAsync();
        if (!employees.Any())
        {
            Console.WriteLine("No employees found.");
        }
        else
        {
            foreach (var employee in employees)
            {
                Console.WriteLine($"ID: {employee.Id}, Name: {employee.FirstName} {employee.LastName}, Email: {employee.Email}, Role: {employee.Role.RoleName}");
            }
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private async Task AddNewEmployeeAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Employee ===");

        Console.Write("Enter First Name: ");
        var firstName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(firstName))
        {
            Console.WriteLine("Error: First Name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Last Name: ");
        var lastName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Error: Last Name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Email: ");
        var email = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Error: Email is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Display available roles
        Console.WriteLine("\n=== Available Roles ===");
        var roles = await _employeeService.GetAllRolesAsync();
        foreach (var role in roles)
        {
            Console.WriteLine($"ID: {role.Id}, Name: {role.RoleName}");
        }

        Console.Write("Enter Role ID: ");
        if (!int.TryParse(Console.ReadLine(), out int roleId))
        {
            Console.WriteLine("Invalid Role ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new EmployeeRegistrationForm
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            RoleId = roleId
        };

        var employee = await _employeeService.RegisterEmployeeAsync(form);
        Console.WriteLine(employee != null ? "Employee added successfully!" : "Error adding employee.");
        Console.ReadKey();
    }

    private async Task UpdateEmployeeAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Employee ===");

        // Step 1: Prompt user to enter the numeric employee ID
        Console.Write("Enter Employee ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int employeeId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Retrieve the existing employee
        var existingEmployee = await _employeeService.GetEmployeeByIdAsync(employeeId);
        if (existingEmployee == null)
        {
            Console.WriteLine("Employee not found. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Step 2: Pre-fill data and allow user to modify fields
        Console.Write($"Enter New First Name (Current: {existingEmployee.FirstName}): ");
        var firstName = Console.ReadLine()?.Trim();
        firstName = string.IsNullOrWhiteSpace(firstName) ? existingEmployee.FirstName : firstName;

        Console.Write($"Enter New Last Name (Current: {existingEmployee.LastName}): ");
        var lastName = Console.ReadLine()?.Trim();
        lastName = string.IsNullOrWhiteSpace(lastName) ? existingEmployee.LastName : lastName;

        Console.Write($"Enter New Email (Current: {existingEmployee.Email}): ");
        var email = Console.ReadLine()?.Trim();
        email = string.IsNullOrWhiteSpace(email) ? existingEmployee.Email : email;

        // Display available roles
        Console.WriteLine("\n=== Available Roles ===");
        var roles = await _employeeService.GetAllRolesAsync();
        foreach (var role in roles)
        {
            Console.WriteLine($"ID: {role.Id}, Name: {role.RoleName}");
        }

        Console.Write($"Select Role ID (Current: {existingEmployee.RoleId}): ");
        if (!int.TryParse(Console.ReadLine(), out int roleId))
        {
            roleId = existingEmployee.RoleId;
        }

        var form = new EmployeeRegistrationForm
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            RoleId = roleId
        };

        var updatedEmployee = await _employeeService.UpdateEmployeeAsync(form, employeeId);
        Console.WriteLine(updatedEmployee != null ? "Employee updated successfully!" : "Error updating employee.");
        Console.ReadKey();
    }


    private async Task DeleteEmployeeAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Employee ===");

        Console.Write("Enter Employee ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int employeeId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var success = await _employeeService.DeleteEmployeeAsync(employeeId);
        Console.WriteLine(success ? "Employee deleted successfully!" : "Employee not found.");
        Console.ReadKey();
    }
}
