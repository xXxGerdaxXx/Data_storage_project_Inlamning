using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace Presentation.MenuService.EntityMenus;

public class ServiceMenu
{
    private readonly IServiceService _serviceService;

    public ServiceMenu(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Service Management ===");
            Console.WriteLine("1. View All Services");
            Console.WriteLine("2. Add New Service");
            Console.WriteLine("3. Update Service");
            Console.WriteLine("4. Delete Service");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ViewAllServicesAsync();
                    break;
                case "2":
                    await AddNewServiceAsync();
                    break;
                case "3":
                    await UpdateServiceAsync();
                    break;
                case "4":
                    await DeleteServiceAsync();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ViewAllServicesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Services ===");

        var services = await _serviceService.GetAllServicesAsync();
        if (services == null || !services.Any()) 
        {
            Console.WriteLine("No services found.");
        }
        else
        {
            foreach (var service in services)
            {
                Console.WriteLine($"ID: {service?.Id}, Name: {service?.ServiceName ?? "Unknown"}, Price: {service?.Price.ToString("C") ?? "N/A"}");
            }
        }

        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    private async Task AddNewServiceAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Service ===");

        Console.Write("Enter Service Name: ");
        var serviceName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(serviceName))
        {
            Console.WriteLine("Error: Service name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
        {
            Console.WriteLine("Invalid price. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Currency ID: ");
        if (!int.TryParse(Console.ReadLine(), out int currencyId))
        {
            Console.WriteLine("Invalid Currency ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new ServiceRegistrationForm
        {
            ServiceName = serviceName,
            Price = price,
            CurrencyId = currencyId
        };

        var service = await _serviceService.RegisterServiceAsync(form);
        Console.WriteLine(service != null ? "Service added successfully!" : "Error adding service.");
        Console.ReadKey();
    }

    private async Task UpdateServiceAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Service ===");

        Console.Write("Enter Service ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int serviceId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Service Name: ");
        var serviceName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(serviceName))
        {
            Console.WriteLine("Error: Service name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
        {
            Console.WriteLine("Invalid price. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Currency ID: ");
        if (!int.TryParse(Console.ReadLine(), out int currencyId))
        {
            Console.WriteLine("Invalid Currency ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new ServiceRegistrationForm
        {
            ServiceName = serviceName,
            Price = price,
            CurrencyId = currencyId
        };

        var updatedService = await _serviceService.UpdateServiceAsync(serviceId, form);
        Console.WriteLine(updatedService != null ? "Service updated successfully!" : "Error updating service.");
        Console.ReadKey();
    }

    private async Task DeleteServiceAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Service ===");

        Console.Write("Enter Service ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int serviceId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var success = await _serviceService.DeleteServiceAsync(serviceId);
        Console.WriteLine(success ? "Service deleted successfully!" : "Service not found.");
        Console.ReadKey();
    }
}
