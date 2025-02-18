using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace Presentation.MenuService.EntityMenus;

public class CustomerMenu(ICustomerService customerService)
{
    private readonly ICustomerService _customerService = customerService;

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
        while (true) 
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
                    Console.WriteLine($"ID: {customer.Id}, Name: {customer.CustomerName}");
                }
            }

            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. View Customer Contact Details");
            Console.WriteLine("2. Back to Main Menu");
            Console.Write("\nEnter your choice: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ViewCustomerContactAsync();
                    break;
                case "2":
                    return; 
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again...");
                    Console.ReadKey();
                    break;
            }
        }
    }
    private async Task ViewCustomerContactAsync()
    {
        Console.Write("\nEnter the Customer ID to view contact details: ");

        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var customer = await _customerService.GetCustomerByIdAsync(customerId);

        if (customer == null)
        {
            Console.WriteLine("Customer not found. Press any key to return...");
            Console.ReadKey();
            return;
        }

       
        Console.Clear();
        Console.WriteLine($"=== Contact Details for {customer.CustomerName} ===");

        if (customer.CustomerContacts == null || customer.CustomerContacts.Count == 0)
        {
            Console.WriteLine("No contact details found for this customer.");
        }
        else
        {
            foreach (var contact in customer.CustomerContacts)
            {
                Console.WriteLine($"First Name: {contact.FirstName}");
                Console.WriteLine($"Last Name: {contact.LastName}");
                Console.WriteLine($"Email: {contact.Email}");
                Console.WriteLine($"Phone: {contact.Phone}\n");
            }
        }

        Console.WriteLine("Press any key to return to the customer list...");
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
        
        Console.Write("Enter First Name: ");
        var firstName = Console.ReadLine()?.Trim();

        Console.Write("Enter Last Name: ");
        var lastName = Console.ReadLine()?.Trim();

        Console.Write("Enter Email: ");
        var email = Console.ReadLine()?.Trim() ?? string.Empty;  

        Console.Write("Enter Phone Number: ");
        var phoneNumber = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Error: First name and last name are required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phoneNumber))
        {
            Console.WriteLine("Error: At least one contact method (email or phone) is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new CustomerRegistrationForm
        {
            CustomerName = customerName,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber
        };

        var customer = await _customerService.RegisterCustomerAsync(form);
        Console.WriteLine(customer != null ? "Customer added successfully!" : "Error adding customer.");
        Console.ReadKey();
    }

    private async Task UpdateCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Customer ===");

        var customers = await _customerService.GetAllCustomersAsync();
        if (customers == null || !customers.Any())
        {
            Console.WriteLine("No customers found.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nAvailable Customers:");
        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.Id}, Name: {customer.CustomerName}");
        }

        Console.Write("\nEnter Customer ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var existingCustomer = await _customerService.GetCustomerByIdAsync(customerId);
        if (existingCustomer == null)
        {
            Console.WriteLine("Customer not found. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write($"Enter New Customer Name (Current: {existingCustomer.CustomerName}): ");
        var customerName = Console.ReadLine()?.Trim();
        customerName = string.IsNullOrWhiteSpace(customerName) ? existingCustomer.CustomerName : customerName;

        var contact = existingCustomer.CustomerContacts.FirstOrDefault();

        Console.Write($"Enter New First Name (Current: {contact?.FirstName ?? "None"}): ");
        var firstName = Console.ReadLine()?.Trim();
        firstName = string.IsNullOrWhiteSpace(firstName) ? contact?.FirstName ?? "" : firstName;

        Console.Write($"Enter New Last Name (Current: {contact?.LastName ?? "None"}): ");
        var lastName = Console.ReadLine()?.Trim();
        lastName = string.IsNullOrWhiteSpace(lastName) ? contact?.LastName ?? "" : lastName;

        Console.Write($"Enter New Email (Current: {contact?.Email ?? "None"}): ");
        var email = Console.ReadLine()?.Trim();
        email = string.IsNullOrWhiteSpace(email) ? contact?.Email ?? "" : email;

        Console.Write($"Enter New Phone Number (Current: {contact?.Phone ?? "None"}): ");
        var phone = Console.ReadLine()?.Trim();
        phone = string.IsNullOrWhiteSpace(phone) ? contact?.Phone ?? "" : phone;

        Console.Write("\nDo you want to save these changes? (Y/N): ");
        var confirmation = Console.ReadLine()?.Trim().ToUpper();

        if (confirmation != "Y")
        {
            Console.WriteLine("Changes discarded. Returning to menu...");
            Console.ReadKey();
            return;
        }

        var form = new CustomerRegistrationForm
        {
            CustomerName = customerName,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phone
        };

        var updatedCustomer = await _customerService.UpdateCustomerAsync(customerId, form);
        Console.WriteLine(updatedCustomer != null ? "Customer updated successfully!" : "Error updating customer.");
        Console.ReadKey();
    }



    private async Task DeleteCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Customer ===");

        var customers = await _customerService.GetAllCustomersAsync();
        if (customers == null || !customers.Any())
        {
            Console.WriteLine("No customers found.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nAvailable Customers:");
        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.Id}, Name: {customer.CustomerName}");
        }

        Console.Write("\nEnter Customer ID to delete: ");
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
