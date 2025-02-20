using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface ICustomerRepository : IBaseRepository<CustomerEntity>
{
    Task<CustomerEntity?> GetByIdAsync(int customerId);
}
