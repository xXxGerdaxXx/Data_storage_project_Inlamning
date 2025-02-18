using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Factories
{
    public static class RoleFactory
    {
        public static RoleDto Create(RoleEntity entity)
        {
            return new RoleDto
            {
                Id = entity.Id,
                RoleName = entity.RoleName
            };
        }
    }
}
