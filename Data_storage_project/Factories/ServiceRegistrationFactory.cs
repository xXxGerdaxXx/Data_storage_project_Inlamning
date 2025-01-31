using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Factories;

public static class ServiceRegistrationFactory
{
    public static ServiceEntity CreateService(ServiceRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Registration form cannot be null.");
        if (string.IsNullOrWhiteSpace(form.ServiceName))
            throw new ArgumentException("Service name is required.", nameof(form.ServiceName));

        return new ServiceEntity
        {
            ServiceName = form.ServiceName,
            Price = form.Price
        };
    }
}
