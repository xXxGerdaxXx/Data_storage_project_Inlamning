using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class CustomerRepository(ApplicationDbContext context) : BaseRepository<CustomerEntity>(context)
{
    public override async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        var entitites = await _context.Customers.Include(x => x.CustomerContacts).ToListAsync();
        return entitites;
    }
}
