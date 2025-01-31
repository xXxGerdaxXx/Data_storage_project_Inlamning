using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace Presentation.MenuService.EntityMenus;

public class CustomerMenu
{
    private readonly ICustomerService _customerService;

    public CustomerMenu(ICustomerService customerService)
    {
        _customerService = customerService; 
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Customer Management ===");
            Console.WriteLine("1. View All Customers");
            Console.WriteLine("2. Add New Customer");
            Console.WriteLine("3. Update Customer");
            Console.WriteLine("4. Delete Customer");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ViewAllCustomersAsync();
                    break;
                case "2":
                    await AddNewCustomerAsync();
                    break;
                case "3":
                    await UpdateCustomerAsync();
                    break;
                case "4":
                    await DeleteCustomerAsync();
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

    private async Task ViewAllCustomersAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Customers ===");

        var customers = await _customerService.GetAllCustomersAsync();
        if (customers == null || !customers.Any()) 
        {
            Console.WriteLine("No customers found.");
        }
        else
        {
            foreach (var customer in customers)
            {
                Console.WriteLine($"ID: {customer?.Id}, Name: {customer?.CustomerName ?? "Unknown"}");
            }
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private async Task AddNewCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Customer ===");

        Console.Write("Enter Customer Name: ");
        var customerName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(customerName))
        {
            Console.WriteLine("Error: Customer name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new CustomerRegistrationForm
        {
            CustomerName = customerName
        };

        var customer = await _customerService.RegisterCustomerAsync(form);
        Console.WriteLine(customer != null ? "Customer added successfully!" : "Error adding customer.");
        Console.ReadKey();
    }

    private async Task UpdateCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Customer ===");

        Console.Write("Enter Customer ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter new Customer Name: ");
        var customerName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(customerName))
        {
            Console.WriteLine("Error: Customer name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new CustomerRegistrationForm
        {
            CustomerName = customerName
        };

        var updatedCustomer = await _customerService.UpdateCustomerAsync(customerId, form);
        Console.WriteLine(updatedCustomer != null ? "Customer updated successfully!" : "Error updating customer.");
        Console.ReadKey();
    }

    private async Task DeleteCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Customer ===");

        Console.Write("Enter Customer ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var success = await _customerService.DeleteCustomerAsync(customerId);
        Console.WriteLine(success ? "Customer deleted successfully!" : "Customer not found.");
        Console.ReadKey();
    }
}
