using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Mappers
{
    public static class CustomerContactMapper
    {
        public static CustomerContactDto ToDto(CustomerContactEntity entity)
        {
            return new CustomerContactDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                Phone = entity.Phone
            };
        }
    }
}
