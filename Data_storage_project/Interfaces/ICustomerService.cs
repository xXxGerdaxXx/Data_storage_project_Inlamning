using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> RegisterCustomerAsync(CustomerRegistrationForm form);
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(int customerId);
    Task<CustomerDto?> UpdateCustomerAsync(int customerId, CustomerUpdateForm form);
    Task<bool> DeleteCustomerAsync(int customerId);

    Task<CustomerDto?> AddCustomerContactAsync(int customerId, CustomerContactDto contactDto);
    Task<CustomerDto?> UpdateCustomerContactAsync(int customerId, int contactId, CustomerContactDto contactDto);
    Task<bool> DeleteCustomerContactAsync(int customerId, int contactId);

}

