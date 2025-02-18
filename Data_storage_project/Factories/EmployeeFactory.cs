using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Factories
{
    public static class EmployeeFactory
    {
        public static EmployeeDto Create(EmployeeEntity entity)
        {
            return new EmployeeDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                RoleId = entity.RoleId,
                RoleName = entity.Role.RoleName
            };
        }
    }
}
