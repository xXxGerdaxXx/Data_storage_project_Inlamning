using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface ICustomerContactRepository : IBaseRepository<CustomerContactEntity>
{
    Task<CustomerContactEntity?> GetByIdAsync(int contactId);
}
