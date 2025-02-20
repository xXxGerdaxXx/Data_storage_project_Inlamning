using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class CustomerRepository(ApplicationDbContext context, ILoggerService logger)
    : BaseRepository<CustomerEntity>(context, logger), ICustomerRepository
{
    public override async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return await _dbSet.Include(c => c.CustomerContacts).ToListAsync();
    }

    public async Task<CustomerEntity?> GetByIdAsync(int customerId)
    {
        return await _dbSet.Include(c => c.CustomerContacts)
            .FirstOrDefaultAsync(c => c.Id == customerId);
    }
}

