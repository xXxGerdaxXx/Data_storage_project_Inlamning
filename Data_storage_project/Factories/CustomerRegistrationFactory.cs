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
            throw new ArgumentException("Customer name is required.", nameof(form.CustomerName));

        var customer = new CustomerEntity
        {
            CustomerName = form.CustomerName,
            CustomerContacts = [] 
        };

        // Creates the CustomerContactEntity and link it to Customer
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
