using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Repositories;

public class StatusTypeRepository : BaseRepository<StatusTypeEntity>
{
    private readonly ApplicationDbContext _context;



    public StatusTypeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


}
