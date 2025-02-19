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

    public async Task<CustomerDto?> RegisterCustomerAsync(CustomerRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Customer form cannot be null.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
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
            await _context.SaveChangesAsync(); 

            await transaction.CommitAsync();
            return createdCustomer != null ? CustomerFactory.Create(createdCustomer) : null;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        return customer != null ? CustomerFactory.Create(customer) : null;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(CustomerFactory.Create);
    }

    public async Task<CustomerDto?> UpdateCustomerAsync(int customerId, CustomerUpdateForm form)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
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

            await _customerRepository.UpdateCustomerAsync(existingCustomer);
            await _context.SaveChangesAsync(); 

            await transaction.CommitAsync();
            return CustomerFactory.Create(existingCustomer);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var customer = await _context.Customers
                .Include(c => c.CustomerContacts)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


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

    public async Task<CustomerDto?> UpdateCustomerContactAsync(int customerId, int contactId, CustomerContactDto contactDto)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
            return null; 

        var contact = customer.CustomerContacts.FirstOrDefault(c => c.Id == contactId);
        if (contact == null)
            return null; 

        contact.FirstName = contactDto.FirstName;
        contact.LastName = contactDto.LastName;
        contact.Email = contactDto.Email;
        contact.Phone = contactDto.Phone;

        await _context.SaveChangesAsync();

        return CustomerFactory.Create(customer);
    }

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
