using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class StatusRepository(ApplicationDbContext context, ILoggerService logger)
    : BaseRepository<StatusTypeEntity>(context, logger), IStatusRepository
{
    public async Task<StatusTypeEntity?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.Name == name);
    }
}
