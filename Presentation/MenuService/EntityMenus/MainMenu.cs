using Data_storage_project_library.Contexts;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using Data_storage_project_library.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Presentation.MenuService.EntityMenus;
using System.Runtime.CompilerServices;

namespace Presentation.MenuService.EntityMenus;

public class MainMenu
{
    private readonly CustomerMenu _customerMenu;
    private readonly EmployeeMenu _employeeMenu;
    private readonly RoleMenu _roleMenu;
    private readonly ServiceMenu _serviceMenu;
    private readonly ProjectMenu _projectMenu;
    private readonly CurrencyMenu _currencyMenu;
    private readonly StatusMenu _statusMenu;

    public MainMenu(CustomerMenu customerMenu, EmployeeMenu employeeMenu, RoleMenu roleMenu, ProjectMenu projectMenu, CurrencyMenu currencyMenu, ServiceMenu serviceMenu, StatusMenu statusMenu)
    {
        _customerMenu = customerMenu;
        _employeeMenu = employeeMenu;
        _roleMenu = roleMenu;
        _projectMenu = projectMenu;
        _currencyMenu = currencyMenu;
        _serviceMenu = serviceMenu;
        _statusMenu = statusMenu;

    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Data Storage Project Menu ===");
            Console.WriteLine("1. Manage Customers");
            Console.WriteLine("2. Manage Employees");
            Console.WriteLine("3. Manage Projects");
            Console.WriteLine("4. Manage Statuses");
            Console.WriteLine("5. Manage Currencies");
            Console.WriteLine("6. Manage Services");
            Console.WriteLine("7. Manage Roles");
            Console.WriteLine("8. Exit");
            Console.Write("Enter choice: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                   await  _customerMenu.RunAsync();
                    
                    break;
                case "2":
                   await _employeeMenu.RunAsync();
                    break;
                case "3":
                    await _projectMenu.RunAsync();
                    break;
                case "4":
                    await _statusMenu.RunAsync();
                    break;
                case "5":
                    await _currencyMenu.RunAsync();
                    break;
                case "6":
                    await _serviceMenu.RunAsync();
                    break;
                case "7":
                    await _roleMenu.RunAsync();
                    break;
                case "8":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
