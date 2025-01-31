using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface ICustomerService
{
    Task<CustomerEntity?> RegisterCustomerAsync(CustomerRegistrationForm form);
    Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync();
    Task<CustomerEntity?> GetCustomerByIdAsync(int customerId);
    Task<CustomerEntity?> UpdateCustomerAsync(int customerId, CustomerRegistrationForm form);
    Task<bool> DeleteCustomerAsync(int customerId);
}

