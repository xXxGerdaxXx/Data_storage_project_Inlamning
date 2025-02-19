using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Repositories;

namespace Data_storage_project_library.Factories;

public static class CustomerFactory
{
    public static CustomerDto Create(CustomerEntity entity)
    {
        return new CustomerDto
        {
            Id = entity.Id,
            CustomerName = entity.CustomerName,

            CustomerContacts = entity.CustomerContacts.Select(x => new CustomerContactDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Phone = x.Phone
            }).ToList()
        };
    }
}
