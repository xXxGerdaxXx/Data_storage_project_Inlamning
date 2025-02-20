using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Mappers;

namespace Data_storage_project_library.Services;

public class CustomerContactService(ICustomerRepository customerRepository, ICustomerContactRepository contactRepository, IUnitOfWork unitOfWork)
    : ICustomerContactService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly ICustomerContactRepository _contactRepository = contactRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CustomerDto?> AddCustomerContactAsync(int customerId, CustomerContactDto contactDto)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                return null;

            var newContact = new CustomerContactEntity
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Email = contactDto.Email,
                Phone = contactDto.Phone
            };

            customer.CustomerContacts.Add(newContact);
            var updatedCustomer = await _customerRepository.UpdateAsync(customer, c => c.Id == customerId);

            return updatedCustomer != null ? CustomerFactory.Create(updatedCustomer) : null;
        });
    }

    public async Task<CustomerContactDto?> GetContactByIdAsync(int contactId)
    {
        var contact = await _contactRepository.GetByIdAsync(contactId);
        return contact != null ? CustomerContactMapper.ToDto(contact) : null;
    }

    public async Task<IEnumerable<CustomerContactDto>> GetAllContactsForCustomerAsync(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

        return customer.CustomerContacts.Select(CustomerContactMapper.ToDto);
    }

    public async Task<CustomerDto?> UpdateCustomerContactAsync(int customerId, int contactId, CustomerContactDto contactDto)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                return null;

            var contact = customer.CustomerContacts.FirstOrDefault(c => c.Id == contactId);
            if (contact == null)
                return null;

            contact.FirstName = contactDto.FirstName;
            contact.LastName = contactDto.LastName;
            contact.Email = contactDto.Email;
            contact.Phone = contactDto.Phone;

            var updatedCustomer = await _customerRepository.UpdateAsync(customer, c => c.Id == customerId);
            return updatedCustomer != null ? CustomerFactory.Create(updatedCustomer) : null;
        });
    }

    public async Task<bool> DeleteCustomerContactAsync(int customerId, int contactId)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                return false;

            var contact = customer.CustomerContacts.FirstOrDefault(c => c.Id == contactId);
            if (contact == null)
                return false;

            customer.CustomerContacts.Remove(contact);
            await _customerRepository.UpdateAsync(customer, c => c.Id == customerId);
            return true;
        });
    }
}
