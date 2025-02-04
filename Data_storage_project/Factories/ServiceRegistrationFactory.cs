using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Factories;

public static class ServiceRegistrationFactory
{
    public static ServiceEntity CreateService(ServiceRegistrationForm form, CurrencyEntity currency)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Registration form cannot be null.");
        if (string.IsNullOrWhiteSpace(form.ServiceName))
            throw new ArgumentException("Service name is required.", nameof(form.ServiceName));
        if (currency == null)
            throw new ArgumentNullException(nameof(currency), "Currency cannot be null.");

        return new ServiceEntity
        {
            ServiceName = form.ServiceName,
            Price = form.Price,
            CurrencyId = currency.Id, // ✅ Use the passed currency object
            Currency = currency        // ✅ Use the passed currency object
        };
    }
}
