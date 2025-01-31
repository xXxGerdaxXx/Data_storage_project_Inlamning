using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Factories;

public static class StatusRegistrationFactory
{
    public static StatusEntity CreateStatus(StatusRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Status registration form cannot be null.");

        return new StatusEntity
        {
            Name = form.Name
        };
    }
}
