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

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
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

            var createdCustomer = await _customerRepository.CreateAsync(newCustomer);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return createdCustomer != null ? CustomerFactory.Create(createdCustomer) : null;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Retrieves a customer by ID and returns a DTO.
    /// </summary>
    public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

        return CustomerFactory.Create(customer);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(CustomerFactory.Create);
            //.Select(ConvertToDto)
            //.Where(dto => dto != null) 
            //.Cast<CustomerDto>(); 
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
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<CustomerDto?> UpdateCustomerAsync(int customerId, CustomerRegistrationForm form)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingCustomer = await _context.Customers
               .Include(c => c.CustomerContacts)
               .FirstOrDefaultAsync(c => c.Id == customerId)
               ?? throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

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

            var updatedCustomer = await _customerRepository.UpdateAsync(existingCustomer, c => c.Id == customerId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return updatedCustomer != null ?  CustomerFactory.Create(updatedCustomer) :  null;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    //private static CustomerDto? ConvertToDto(CustomerEntity? entity)
    //{
    //    if (entity == null)
    //        return null;

    //    return new CustomerDto
    //    {
    //        Id = entity.Id,
    //        CustomerName = entity.CustomerName,
    //        CustomerContact = entity.CustomerContacts?.FirstOrDefault() != null ? new CustomerContactDto
    //        {
    //            Id = entity.CustomerContacts.First().Id,
    //            FirstName = entity.CustomerContacts.First().FirstName,
    //            LastName = entity.CustomerContacts.First().LastName,
    //            Email = entity.CustomerContacts.First().Email,
    //            Phone = entity.CustomerContacts.First().Phone
    //        } : null
    //    };
    //}

}
