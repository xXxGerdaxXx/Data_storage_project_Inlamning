using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class CurrencyRepository(ApplicationDbContext context) : BaseRepository<CurrencyEntity>(context)
{
    private readonly ApplicationDbContext _context = context;
}
