using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_storage_project_library.Services;

public class CustomerService(CustomerRepository customerRepository, ApplicationDbContext context) : ICustomerService
{
    private readonly CustomerRepository _customerRepository = customerRepository;
    private readonly ApplicationDbContext _context = context;

    /// <summary>
    /// Registers a new customer and returns the created CustomerDto.
    /// </summary>
    public async Task<CustomerDto?> RegisterCustomerAsync(CustomerRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Customer form cannot be null.");

        var newCustomer = new CustomerEntity
        {
            CustomerName = form.CustomerName,
            CustomerContacts = form.CustomerContacts
                .Select(contact => new CustomerContactEntity
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Phone = contact.Phone
                }).ToList()
        };

        var createdCustomer = await _customerRepository.CreateCustomerAsync(newCustomer);
        return createdCustomer != null ? CustomerFactory.Create(createdCustomer) : null;
    }

    /// <summary>
    /// Retrieves a customer by ID and returns a DTO.
    /// </summary>
    public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        return customer != null ? CustomerFactory.Create(customer) : null;
    }

    /// <summary>
    /// Retrieves all customers and their contacts.
    /// </summary>
    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(CustomerFactory.Create);
    }

    /// <summary>
    /// Updates a customer and their contact information.
    /// </summary>
    public async Task<CustomerDto?> UpdateCustomerAsync(int customerId, CustomerUpdateForm form)
    {
        var existingCustomer = await _context.Customers
           .Include(c => c.CustomerContacts)
           .FirstOrDefaultAsync(c => c.Id == customerId)
           ?? throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

        existingCustomer.CustomerName = form.CustomerName;

        foreach (var contactDto in form.CustomerContacts)
        {
            var existingContact = existingCustomer.CustomerContacts
                .FirstOrDefault(c => c.Id == contactDto.Id);

            if (existingContact != null)
            {
                existingContact.FirstName = contactDto.FirstName;
                existingContact.LastName = contactDto.LastName;
                existingContact.Email = contactDto.Email;
                existingContact.Phone = contactDto.Phone;
            }
            else
            {
                existingCustomer.CustomerContacts.Add(new CustomerContactEntity
                {
                    FirstName = contactDto.FirstName,
                    LastName = contactDto.LastName,
                    Email = contactDto.Email,
                    Phone = contactDto.Phone
                });
            }
        }

        var updatedCustomer = await _customerRepository.UpdateCustomerAsync(existingCustomer);
        return updatedCustomer != null ? CustomerFactory.Create(updatedCustomer) : null;
    }

    /// <summary>
    /// Deletes a customer and their associated contacts.
    /// </summary>
    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        return await _customerRepository.DeleteCustomerAsync(customerId);
    }

    /// <summary>
    /// Adds a new contact to a customer.
    /// </summary>
    public async Task<CustomerDto?> AddCustomerContactAsync(int customerId, CustomerContactDto contactDto)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
            return null;

        var newContact = new CustomerContactEntity
        {
            FirstName = contactDto.FirstName,
            LastName = contactDto.LastName,
            Email = contactDto.Email,
            Phone = contactDto.Phone
        };

        customer.CustomerContacts.Add(newContact);
        await _context.SaveChangesAsync();

        return CustomerFactory.Create(customer); 
    }


    /// <summary>
    /// Updates an existing customer contact.
    /// </summary>
    public async Task<CustomerDto?> UpdateCustomerContactAsync(int customerId, int contactId, CustomerContactDto contactDto)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
            return null; // Customer not found

        var contact = customer.CustomerContacts.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            return null; // Contact not found

        // Update contact details
        contact.FirstName = contactDto.FirstName;
        contact.LastName = contactDto.LastName;
        contact.Email = contactDto.Email;
        contact.Phone = contactDto.Phone;

        await _context.SaveChangesAsync();

        // Return updated customer as DTO
        return CustomerFactory.Create(customer);
    }

    /// <summary>
    /// Deletes a contact from a customer.
    /// </summary>
    public async Task<bool> DeleteCustomerContactAsync(int customerId, int contactId)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
            return false;

        var contact = customer.CustomerContacts.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            return false;

        customer.CustomerContacts.Remove(contact);
        await _context.SaveChangesAsync();
        return true;
    }
}
