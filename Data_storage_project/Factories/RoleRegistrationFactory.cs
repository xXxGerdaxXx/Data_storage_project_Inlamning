using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Factories;

public static class RoleRegistrationFactory
{
    public static RoleEntity CreateRole(RoleRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Role registration form cannot be null.");

        return new RoleEntity
        {
            RoleName = form.RoleName
        };
    }
}
