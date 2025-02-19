using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Repositories
{
    public class ProjectRepository(ApplicationDbContext context) : BaseRepository<ProjectEntity>(context)
    {
        

        public override async Task<IEnumerable<ProjectEntity>> GetAllAsync()
        {
            Console.WriteLine("Fetching all projects...");

            var entities = await _context.Projects
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .Include(x => x.Status)
                .Include(x => x.Service)
                .ToListAsync();

            Console.WriteLine($"Total projects fetched: {entities.Count}");

            return entities;
        }


        public async Task<ProjectEntity?> GetLastProjectAsync()
        {
            return await _context.Projects
                .OrderByDescending(p => EF.Functions.Like(p.Id, "P-%") ? Convert.ToInt32(p.Id.Substring(2)) : 0)
                .FirstOrDefaultAsync();
        }

    }
}
