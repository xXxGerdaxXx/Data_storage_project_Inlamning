using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class ServiceRepository(ApplicationDbContext context) : BaseRepository<ServiceEntity>(context)
{
    private readonly ApplicationDbContext _context = context;

    //public async Task<IEnumerable<ServiceEntity>> GetAllServicesAsync()
    //{
    //    // Use the base repository's GetAllAsync with eager loading for Currency
    //    return await GetAllAsync(s => s.Currency);
    //}
}
