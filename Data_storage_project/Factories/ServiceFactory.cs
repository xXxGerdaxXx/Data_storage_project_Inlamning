using Data_storage_project_library.Entities;
using Data_storage_project_library.Dtos;

namespace Data_storage_project_library.Factories;

public static class ServiceFactory
{
    public static ServiceEntity CreateService(ServiceRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Service form cannot be null.");

        return new ServiceEntity
        {
            ServiceName = form.ServiceName,
            Price = form.Price,
            CurrencyId = form.CurrencyId
        };
    }
}
