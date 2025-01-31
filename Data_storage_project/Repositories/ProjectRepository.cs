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
    public class ProjectRepository : BaseRepository<ProjectEntity>
    {
        private readonly ApplicationDbContext _context;

       

        public ProjectRepository(ApplicationDbContext context, DbSet<ProjectEntity> dbSet) : base(context, dbSet)
        {
            _context = context;
        }

        public async Task<ProjectEntity> GetLastProjectAsync()
        {
            var entity = await _context.Projects.LastAsync();
            return entity;

        }
    }
}
