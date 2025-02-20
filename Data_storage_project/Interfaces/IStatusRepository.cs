using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IStatusRepository : IBaseRepository<StatusTypeEntity>
{
    Task<StatusTypeEntity?> GetByNameAsync(string name);
}
