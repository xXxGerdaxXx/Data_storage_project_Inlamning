using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Repositories;

public class CustomerContactRepository(ApplicationDbContext context) : BaseRepository<CustomerContactEntity>(context)
{
}
