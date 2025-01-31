using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Repositories;

public class CurrencyRepository(ApplicationDbContext context) : BaseRepository<CurrencyEntity>(context)
{
    private readonly ApplicationDbContext _context = context;
}
