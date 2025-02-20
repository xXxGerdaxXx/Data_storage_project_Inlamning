using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;

namespace Data_storage_project_library.Repositories;

public class CustomerContactRepository(ApplicationDbContext context, ILoggerService logger) : BaseRepository<CustomerContactEntity>(context, logger)
{
}
