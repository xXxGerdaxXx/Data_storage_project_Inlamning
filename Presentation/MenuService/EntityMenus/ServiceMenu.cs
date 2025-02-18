using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Services;
using Microsoft.EntityFrameworkCore;

namespace Presentation.MenuService.EntityMenus;

public class ServiceMenu(IServiceService serviceService, ICurrencyService currencyService)
{
    private readonly IServiceService _serviceService = serviceService ?? throw new ArgumentNullException(nameof(serviceService));
    private readonly ICurrencyService _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));

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
        if (!services.Any())
        {
            Console.WriteLine("No services found.");
        }
        else
        {
            foreach (var service in services)
            {
                Console.WriteLine($"ID: {service.Id}, Name: {service.ServiceName}, Price per hour: {service.Price:F2} {service.CurrencyCode}");
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

        Console.WriteLine("\nAvailable Currencies:");
        var currencies = await _currencyService.GetAllCurrenciesAsync();
        if (!currencies.Any())
        {
            Console.WriteLine("No currencies found. Press any key to return...");
            Console.ReadKey();
            return;
        }

        foreach (var currency in currencies)
        {
            Console.WriteLine($"ID: {currency.Id}, Code: {currency.Code}, Name: {currency.Name}");
        }

        Console.Write("\nEnter Currency ID: ");
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

        var services = await _serviceService.GetAllServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("No services found.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nAvailable Services:");
        foreach (var service in services)
        {
            Console.WriteLine($"ID: {service.Id}, Name: {service.ServiceName}, Price: {service.Price:F2} {service.CurrencyCode}");
        }

        Console.Write("\nEnter Service ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int serviceId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var existingService = await _serviceService.GetServiceByIdAsync(serviceId);
        if (existingService == null)
        {
            Console.WriteLine("Service not found. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write($"Enter New Service Name (Current: {existingService.ServiceName}): ");
        var serviceName = Console.ReadLine()?.Trim();
        serviceName = string.IsNullOrWhiteSpace(serviceName) ? existingService.ServiceName : serviceName;

        Console.Write($"Enter New Price (Current: {existingService.Price:F2}): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
        {
            price = existingService.Price;
        }

        Console.WriteLine("\nAvailable Currencies:");
        var currencies = await _currencyService.GetAllCurrenciesAsync();
        if (!currencies.Any())
        {
            Console.WriteLine("No currencies found. Press any key to return...");
            Console.ReadKey();
            return;
        }

        foreach (var currency in currencies)
        {
            Console.WriteLine($"ID: {currency.Id}, Code: {currency.Code}, Name: {currency.Name}");
        }

        Console.Write($"Enter New Currency ID (Current: {existingService.CurrencyId}): ");
        if (!int.TryParse(Console.ReadLine(), out int currencyId))
        {
            currencyId = existingService.CurrencyId;
        }

        Console.Write("\nDo you want to save these changes? (Y/N): ");
        var confirmation = Console.ReadLine()?.Trim().ToUpper();

        if (confirmation != "Y")
        {
            Console.WriteLine("Changes discarded. Returning to menu...");
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

        var services = await serviceService.GetAllServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("No services found.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nAvailable Services:");
        foreach (var service in services)
        {
            Console.WriteLine($"ID: {service.Id}, Name: {service.ServiceName}, Price: {service.Price:F2} {service.CurrencyCode}");
        }

        Console.Write("\nEnter Service ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int serviceId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var success = await serviceService.DeleteServiceAsync(serviceId);
        Console.WriteLine(success ? "Service deleted successfully!" : "Service not found.");
        Console.ReadKey();
    }

}
