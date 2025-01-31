using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace Presentation.MenuService.EntityMenus;

public class RoleMenu
{
    private readonly IRoleService _roleService;

    public RoleMenu(IRoleService roleService)
    {
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService)); 
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Role Management ===");
            Console.WriteLine("1. View All Roles");
            Console.WriteLine("2. Add New Role");
            Console.WriteLine("3. Update Role");
            Console.WriteLine("4. Delete Role");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ViewAllRolesAsync();
                    break;
                case "2":
                    await AddNewRoleAsync();
                    break;
                case "3":
                    await UpdateRoleAsync();
                    break;
                case "4":
                    await DeleteRoleAsync();
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

    private async Task ViewAllRolesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Roles ===");

        var roles = await _roleService.GetAllRolesAsync();
        if (roles == null || !roles.Any()) 
        {
            Console.WriteLine("No roles found.");
        }
        else
        {
            foreach (var role in roles)
            {
                Console.WriteLine($"ID: {role?.Id}, Name: {role?.RoleName ?? "Unknown"}"); 
            }
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private async Task AddNewRoleAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Role ===");

        Console.Write("Enter Role Name: ");
        var roleName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(roleName))
        {
            Console.WriteLine("Error: Role name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new RoleRegistrationForm
        {
            RoleName = roleName
        };

        var role = await _roleService.RegisterRoleAsync(form);
        Console.WriteLine(role != null ? "Role added successfully!" : "Error adding role.");
        Console.ReadKey();
    }

    private async Task UpdateRoleAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Role ===");

        Console.Write("Enter Role ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int roleId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Role Name: ");
        var roleName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(roleName))
        {
            Console.WriteLine("Error: Role name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new RoleRegistrationForm
        {
            RoleName = roleName
        };

        var updatedRole = await _roleService.UpdateRoleAsync(roleId, form);
        Console.WriteLine(updatedRole != null ? "Role updated successfully!" : "Error updating role.");
        Console.ReadKey();
    }

    private async Task DeleteRoleAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Role ===");

        Console.Write("Enter Role ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int roleId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var success = await _roleService.DeleteRoleAsync(roleId);
        Console.WriteLine(success ? "Role deleted successfully!" : "Role not found.");
        Console.ReadKey();
    }
}
