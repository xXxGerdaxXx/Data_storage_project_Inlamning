using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class CustomerContactRepository(ApplicationDbContext context, ILoggerService logger)
    : BaseRepository<CustomerContactEntity>(context, logger), ICustomerContactRepository
{
    public async Task<CustomerContactEntity?> GetByIdAsync(int contactId)
    {
        return await _context.CustomerContacts.FirstOrDefaultAsync(c => c.Id == contactId);
    }
}
