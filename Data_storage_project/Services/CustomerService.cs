using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Factories;

namespace Data_storage_project_library.Services;

public class CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CustomerDto?> RegisterCustomerAsync(CustomerRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Customer form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var newCustomer = new CustomerEntity
            {
                CustomerName = form.CustomerName,
                CustomerContacts = form.CustomerContacts.Select(contact => new CustomerContactEntity
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Phone = contact.Phone
                }).ToList()
            };

            var createdCustomer = await _customerRepository.CreateAsync(newCustomer);
            return createdCustomer != null ? CustomerFactory.Create(createdCustomer) : null;
        });
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(CustomerFactory.Create);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        return customer != null ? CustomerFactory.Create(customer) : null;
    }

    public async Task<CustomerDto?> UpdateCustomerAsync(int customerId, CustomerUpdateForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Customer update form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customerId)
                ?? throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            existingCustomer.CustomerName = form.CustomerName;

            foreach (var contactDto in form.CustomerContacts)
            {
                var existingContact = existingCustomer.CustomerContacts.FirstOrDefault(c => c.Id == contactDto.Id);

                if (existingContact != null)
                {
                    existingContact.FirstName = contactDto.FirstName;
                    existingContact.LastName = contactDto.LastName;
                    existingContact.Email = contactDto.Email;
                    existingContact.Phone = contactDto.Phone;
                }
                else
                {
                    existingCustomer.CustomerContacts.Add(new CustomerContactEntity
                    {
                        FirstName = contactDto.FirstName,
                        LastName = contactDto.LastName,
                        Email = contactDto.Email,
                        Phone = contactDto.Phone
                    });
                }
            }

            var updatedCustomer = await _customerRepository.UpdateAsync(existingCustomer, c => c.Id == customerId);
            return updatedCustomer != null ? CustomerFactory.Create(updatedCustomer) : null;
        });
    }

    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            return await _customerRepository.DeleteAsync(c => c.Id == customerId);
        });
    }
}
