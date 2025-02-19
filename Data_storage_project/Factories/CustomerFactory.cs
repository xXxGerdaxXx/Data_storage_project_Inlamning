using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Factories;

public static class CustomerFactory
{
    public static CustomerDto Create(CustomerEntity entity)
    {
        return new CustomerDto
        {
            Id = entity.Id,
            CustomerName = entity.CustomerName,
            CustomerContact = entity.CustomerContacts.Select(x => new CustomerContactDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Phone = x.Phone,

            }).ToList(),
           

            };
        }
}
