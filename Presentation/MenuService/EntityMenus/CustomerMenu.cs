using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Helpers;


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
            Console.WriteLine("5. Manage Customer Contacts");
            Console.WriteLine("6. Back to Main Menu");
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
                    await ManageCustomerContactsAsync();
                    break;
                case "6":
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
        if (!customers.Any())
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

        if (customer.CustomerContacts == null || !customer.CustomerContacts.Any())
        {
            Console.WriteLine("No contact details found for this customer.");
        }
        else
        {
            foreach (var contact in customer.CustomerContacts)
            {
                Console.WriteLine($"Contact ID: {contact.Id}");
                Console.WriteLine($"First Name: {contact.FirstName}");
                Console.WriteLine($"Last Name: {contact.LastName}");
                Console.WriteLine($"Email: {contact.Email}");
                Console.WriteLine($"Phone: {contact.Phone}");
                Console.WriteLine("----------------------");
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

        var contacts = new List<CustomerContactDto>();

        while (true)
        {
            Console.WriteLine("\nEnter Contact Details:");

            Console.Write("First Name: ");
            var firstName = Console.ReadLine()?.Trim();

            Console.Write("Last Name: ");
            var lastName = Console.ReadLine()?.Trim();

            Console.Write("Email: ");
            var email = Console.ReadLine()?.Trim() ?? string.Empty;

            Console.Write("Phone Number: ");
            var phoneNumber = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Error: First name and last name are required.");
                continue;
            }

            contacts.Add(new CustomerContactDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phoneNumber
            });

            Console.Write("\nDo you want to add another contact? (Y/N): ");
            var addAnother = Console.ReadLine()?.Trim().ToUpper();
            if (addAnother != "Y")
                break;
        }

        var form = new CustomerRegistrationForm
        {
            CustomerName = customerName,
            CustomerContacts = contacts
        };

        var customer = await _customerService.RegisterCustomerAsync(form);
        Console.WriteLine(customer != null ? "Customer added successfully!" : "Error adding customer.");
        Console.ReadKey();
    }

    private async Task ManageCustomerContactsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Manage Customer Contacts ===");

        Console.Write("\nEnter Customer ID to manage contacts: ");
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

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Managing Contacts for: {customer.CustomerName}");
            Console.WriteLine("\nExisting Contacts:");

            if (!customer.CustomerContacts.Any())
            {
                Console.WriteLine("No contacts found for this customer.");
            }
            else
            {
                foreach (var contact in customer.CustomerContacts)
                {
                    Console.WriteLine($"Contact ID: {contact.Id}");
                    Console.WriteLine($" - Name: {contact.FirstName} {contact.LastName}");
                    Console.WriteLine($" - Email: {contact.Email}");
                    Console.WriteLine($" - Phone: {contact.Phone}");
                    Console.WriteLine("----------------------");
                }
            }

            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Add a Contact");
            Console.WriteLine("2. Update a Contact");  
            Console.WriteLine("3. Delete a Contact");
            Console.WriteLine("4. Back to Main Menu");
            Console.Write("\nEnter your choice: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await AddContactToCustomerAsync(customerId);
                    break;
                case "2":
                    await UpdateCustomerContactAsync(customerId);  
                    break;
                case "3":
                    await DeleteCustomerContactAsync(customerId);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task UpdateCustomerContactAsync(int customerId)
    {
        Console.Clear();
        Console.WriteLine("=== Update Customer Contact ===");

        Console.Write("\nEnter Contact ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int contactId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var customer = await _customerService.GetCustomerByIdAsync(customerId);
        if (customer == null || !customer.CustomerContacts.Any(c => c.Id == contactId))
        {
            Console.WriteLine("Contact not found. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var contact = customer.CustomerContacts.First(c => c.Id == contactId);

        Console.Write($"Enter New First Name (Current: {contact.FirstName}): ");
        var firstName = Console.ReadLine()?.Trim();
        firstName = string.IsNullOrWhiteSpace(firstName) ? contact.FirstName : firstName;

        Console.Write($"Enter New Last Name (Current: {contact.LastName}): ");
        var lastName = Console.ReadLine()?.Trim();
        lastName = string.IsNullOrWhiteSpace(lastName) ? contact.LastName : lastName;

        Console.Write($"Enter New Email (Current: {contact.Email}): ");
        var email = Console.ReadLine()?.Trim();
        email = string.IsNullOrWhiteSpace(email) ? contact.Email : email;

        Console.Write($"Enter New Phone Number (Current: {contact.Phone}): ");
        var phone = Console.ReadLine()?.Trim();
        phone = string.IsNullOrWhiteSpace(phone) ? contact.Phone : phone;

        var updatedContact = new CustomerContactDto
        {
            Id = contactId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone
        };

        var updatedCustomer = await _customerService.UpdateCustomerContactAsync(customerId, contactId, updatedContact);

        if (updatedCustomer != null)
        {
            Console.WriteLine("Contact updated successfully!");
        }
        else
        {
            Console.WriteLine("Error updating contact.");
        }

        Console.ReadKey();
    }


    private async Task AddContactToCustomerAsync(int customerId)
    {
        Console.Clear();
        Console.WriteLine("=== Add Contact to Customer ===");

        string firstName, lastName, email, phoneNumber;

        do
        {
            Console.Write("First Name (Required): ");
            firstName = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(firstName))
                Console.WriteLine("❌ Error: First Name cannot be empty. Please enter a valid name.");
        } while (string.IsNullOrWhiteSpace(firstName));

        do
        {
            Console.Write("Last Name (Required): ");
            lastName = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(lastName))
                Console.WriteLine("❌ Error: Last Name cannot be empty. Please enter a valid name.");
        } while (string.IsNullOrWhiteSpace(lastName));

        do
        {
            Console.Write("Email (Optional, press Enter to skip): ");
            email = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(email) && !ValidationHelper.IsValidEmail(email))
                Console.WriteLine("⚠️ Warning: Invalid email format. Try again or leave it empty.");
            else
                break;
        } while (!string.IsNullOrWhiteSpace(email));

        do
        {
            Console.Write("Phone Number (Optional, press Enter to skip): ");
            phoneNumber = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(phoneNumber) && !ValidationHelper.IsValidPhoneNumber(phoneNumber))
                Console.WriteLine("⚠️ Warning: Invalid phone number format. Try again or leave it empty.");
            else
                break;
        } while (!string.IsNullOrWhiteSpace(phoneNumber));

        var contact = new CustomerContactDto
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phoneNumber
        };

        var updatedCustomer = await _customerService.AddCustomerContactAsync(customerId, contact);
        Console.WriteLine(updatedCustomer != null ? "✅ Contact added successfully!" : "❌ Error adding contact.");
        Console.ReadKey();
    }


    private async Task DeleteCustomerContactAsync(int customerId)
    {
        Console.Write("\nEnter Contact ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int contactId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var success = await _customerService.DeleteCustomerContactAsync(customerId, contactId);
        Console.WriteLine(success ? "Contact deleted successfully!" : "Contact not found.");

        Console.ReadKey();
    }

    private async Task UpdateCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Customer ===");

        var customers = await _customerService.GetAllCustomersAsync();
        if (!customers.Any())
        {
            Console.WriteLine("No customers found.");
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

        var updatedContacts = new List<CustomerContactDto>();

        foreach (var contact in existingCustomer.CustomerContacts)
        {
            Console.WriteLine($"\nUpdating Contact (ID: {contact.Id})");

            Console.Write($"Enter New First Name (Current: {contact.FirstName}): ");
            var firstName = Console.ReadLine()?.Trim();
            firstName = string.IsNullOrWhiteSpace(firstName) ? contact.FirstName : firstName;

            Console.Write($"Enter New Last Name (Current: {contact.LastName}): ");
            var lastName = Console.ReadLine()?.Trim();
            lastName = string.IsNullOrWhiteSpace(lastName) ? contact.LastName : lastName;

            Console.Write($"Enter New Email (Current: {contact.Email}): ");
            var email = Console.ReadLine()?.Trim();
            email = string.IsNullOrWhiteSpace(email) ? contact.Email : email;

            Console.Write($"Enter New Phone Number (Current: {contact.Phone}): ");
            var phone = Console.ReadLine()?.Trim();
            phone = string.IsNullOrWhiteSpace(phone) ? contact.Phone : phone;

            updatedContacts.Add(new CustomerContactDto
            {
                Id = contact.Id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone
            });
        }

        var updateForm = new CustomerUpdateForm
        {
            CustomerName = customerName,
            CustomerContacts = updatedContacts
        };

        var updatedCustomer = await _customerService.UpdateCustomerAsync(customerId, updateForm);
        Console.WriteLine(updatedCustomer != null ? "Customer updated successfully!" : "Error updating customer.");
        Console.ReadKey();
    }



    private async Task DeleteCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Customer ===");

        var customers = await _customerService.GetAllCustomersAsync();
        if (!customers.Any())
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
