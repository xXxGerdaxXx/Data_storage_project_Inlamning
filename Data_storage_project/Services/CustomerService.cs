using Data_storage_project_library.Contexts; 
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Services;

public class CustomerService(CustomerRepository customerRepository, ApplicationDbContext context) : ICustomerService
{
    private readonly CustomerRepository _customerRepository = customerRepository;
    private readonly ApplicationDbContext _context = context; 

    public async Task<CustomerEntity?> RegisterCustomerAsync(CustomerRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Customer form cannot be null.");

        var newCustomer = new CustomerEntity
        {
            CustomerName = form.CustomerName,
            CustomerContacts =
            [
                new()
                {
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    Email = form.Email,
                    Phone = form.PhoneNumber
                }
            ]
        };

        return await _customerRepository.CreateAsync(newCustomer);
    }

    public async Task<CustomerEntity?> GetCustomerByIdAsync(int customerId)
    {
        return await _context.Customers
            .Include(c => c.CustomerContacts) 
            .FirstOrDefaultAsync(c => c.Id == customerId);
    }

    public async Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        return await _customerRepository.DeleteAsync(c => c.Id == customerId);
    }

    public async Task<CustomerEntity?> UpdateCustomerAsync(int customerId, CustomerRegistrationForm form)
    {
        
        var existingCustomer = await _context.Customers
            .Include(c => c.CustomerContacts) 
            .FirstOrDefaultAsync(c => c.Id == customerId) ?? throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
        existingCustomer.CustomerName = form.CustomerName;

        var existingContact = existingCustomer.CustomerContacts.FirstOrDefault();
        if (existingContact != null)
        {
            existingContact.FirstName = form.FirstName;
            existingContact.LastName = form.LastName;
            existingContact.Email = form.Email;
            existingContact.Phone = form.PhoneNumber;
        }
        else
        {
            existingCustomer.CustomerContacts.Add(new CustomerContactEntity
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                Phone = form.PhoneNumber
            });
        }

        return await _customerRepository.UpdateAsync(existingCustomer, c => c.Id == customerId);
    }
}
