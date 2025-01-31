using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Repositories
{
    public class CustomerRepository : BaseRepository<CustomerEntity>
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
