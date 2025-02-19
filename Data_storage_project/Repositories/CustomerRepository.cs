using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Data_storage_project_library.Repositories;

public class CustomerRepository(ApplicationDbContext context) : BaseRepository<CustomerEntity>(context)
{
    public override async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return await _context.Customers.Include(x => x.CustomerContacts).ToListAsync();
    }

    public async Task<CustomerEntity?> CreateCustomerAsync(CustomerEntity customer)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return customer;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<CustomerEntity?> UpdateCustomerAsync(CustomerEntity customer)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return customer;
        }
        catch (Exception)
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
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
