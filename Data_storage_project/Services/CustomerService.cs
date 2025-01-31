using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;

namespace Data_storage_project_library.Services;

public class CustomerService(BaseRepository<CustomerEntity> customerRepository) : ICustomerService
{
    private readonly BaseRepository<CustomerEntity> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

    public async Task<CustomerEntity?> RegisterCustomerAsync(CustomerRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Customer form cannot be null.");

        var newCustomer = CustomerRegistrationFactory.CreateCustomer(form);

        return await _customerRepository.CreateAsync(newCustomer);
    }

    public async Task<CustomerEntity?> GetCustomerByIdAsync(int customerId)
    {
        return await _customerRepository.GetAsync(c => c.Id == customerId);
    }

    public async Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        return await _customerRepository.DeleteAsync(c => c.Id == customerId);
    }
}
