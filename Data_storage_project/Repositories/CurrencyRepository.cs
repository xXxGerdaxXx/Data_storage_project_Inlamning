using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class CurrencyRepository(ApplicationDbContext context, ILoggerService logger)
    : BaseRepository<CurrencyEntity>(context, logger), ICurrencyRepository
{
    public async Task<CurrencyEntity?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Code == code); 
    }
}
