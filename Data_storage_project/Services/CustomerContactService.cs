using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Data_storage_project_library.Services
{
    public class CustomerContactService(ApplicationDbContext context) : ICustomerContactService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<CustomerDto?> AddCustomerContactAsync(int customerId, CustomerContactDto contactDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(); 

            try
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

                await transaction.CommitAsync(); 
                return CustomerFactory.Create(customer);
            }
            catch
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }

        public async Task<CustomerDto?> UpdateCustomerContactAsync(int customerId, int contactId, CustomerContactDto contactDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
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
                await transaction.CommitAsync(); 
                return CustomerFactory.Create(customer);
            }
            catch
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }

        public async Task<bool> DeleteCustomerContactAsync(int customerId, int contactId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
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

                await transaction.CommitAsync(); 
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }
    }
}
