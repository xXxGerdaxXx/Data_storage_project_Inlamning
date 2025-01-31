using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;

namespace Data_storage_project_library.Services;

public class CustomerService(CustomerRepository customerRepository) : ICustomerService
{
    private readonly CustomerRepository _customerRepository = customerRepository;

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

    public async Task<CustomerEntity?> UpdateCustomerAsync(int customerId, CustomerRegistrationForm form)
    {
        var existingCustomer = await _customerRepository.GetAsync(c => c.Id == customerId);
        if (existingCustomer == null)
            throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

        existingCustomer.CustomerName = form.CustomerName;

        return await _customerRepository.UpdateAsync(existingCustomer, c => c.Id == customerId);
    }

}
