using Data_storage_project_library.Dtos;

namespace Data_storage_project_library.Interfaces
{
    public interface ICustomerContactService
    {
        Task<CustomerDto?> AddCustomerContactAsync(int customerId, CustomerContactDto contactDto);
        Task<CustomerDto?> UpdateCustomerContactAsync(int customerId, int contactId, CustomerContactDto contactDto);
        Task<bool> DeleteCustomerContactAsync(int customerId, int contactId);
    }
}
