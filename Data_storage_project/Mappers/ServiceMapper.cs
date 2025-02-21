using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using System.Globalization;

namespace Data_storage_project_library.Mappers;

public static class ServiceMapper
{
    public static ServiceDto ToDto(ServiceEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

        if (entity.Currency == null)
            throw new InvalidOperationException($"Service {entity.Id} is missing a currency. This should never happen.");

        return new ServiceDto
        {
            Id = entity.Id,
            ServiceName = entity.ServiceName,
            Price = entity.Price,
            CurrencyId = entity.CurrencyId,
            CurrencyCode = entity.Currency.Code,
            FormattedPrice = entity.Price.ToString("0.00", CultureInfo.InvariantCulture) 
        };
    }
}
