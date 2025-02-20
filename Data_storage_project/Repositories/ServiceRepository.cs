using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class ServiceRepository(ApplicationDbContext context, ILoggerService logger)
    : BaseRepository<ServiceEntity>(context, logger), IServiceRepository
{
    public async Task<IEnumerable<ServiceEntity>> GetAllWithCurrencyAsync()
    {
        return await _dbSet.Include(s => s.Currency).ToListAsync(); 
    }

    public async Task<ServiceEntity?> GetByIdWithCurrencyAsync(int serviceId)
    {
        return await _dbSet.Include(s => s.Currency) 
            .FirstOrDefaultAsync(s => s.Id == serviceId);
    }
}

