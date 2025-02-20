using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class ProjectRepository(ApplicationDbContext context, ILoggerService logger)
    : BaseRepository<ProjectEntity>(context, logger), IProjectRepository
{
    public async Task<ProjectEntity?> GetByIdAsync(string projectId)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Id == projectId); 
    }

    public override async Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        return await _dbSet 
            .Include(x => x.Customer)
            .Include(x => x.Employee)
            .Include(x => x.Status)
            .Include(x => x.Service)
            .ToListAsync();
    }

    public async Task<ProjectEntity?> GetLastProjectAsync()
    {
        return await _dbSet 
            .OrderByDescending(p => EF.Functions.Like(p.Id, "P-%") ? Convert.ToInt32(p.Id.Substring(2)) : 0)
            .FirstOrDefaultAsync();
    }
}
