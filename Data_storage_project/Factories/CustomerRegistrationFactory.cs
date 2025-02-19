using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Factories;

public static class CustomerRegistrationFactory
{

    public static CustomerEntity CreateCustomer(CustomerRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Registration form cannot be null.");

        if (string.IsNullOrWhiteSpace(form.CustomerName))
            throw new ArgumentException("Customer name is required.", nameof(form));

        if (string.IsNullOrWhiteSpace(form.FirstName) ||
            string.IsNullOrWhiteSpace(form.LastName) ||
            string.IsNullOrWhiteSpace(form.Email) ||
            string.IsNullOrWhiteSpace(form.PhoneNumber))
        {
            throw new ArgumentException("Contact details (First Name, Last Name, Email, Phone) are required.");
        }

        var customer = new CustomerEntity
        {
            CustomerName = form.CustomerName,
            CustomerContacts = new List<CustomerContactEntity>() 
        };

        // Creates the CustomerContactEntity and links it to the Customer
        var contact = new CustomerContactEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            Phone = form.PhoneNumber
        };

        // Links the CustomerContact to the customer
        customer.CustomerContacts.Add(contact);

        return customer;
    }
}
