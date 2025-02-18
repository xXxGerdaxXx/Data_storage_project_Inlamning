using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Factories;

public static class ServiceFactory
{
    public static ServiceDto Create(ServiceEntity entity)
    {
        if (entity.Currency == null)
            throw new InvalidOperationException($"Service {entity.Id} is missing a currency. This should never happen.");

        return new ServiceDto
        {
            Id = entity.Id,
            ServiceName = entity.ServiceName,
            Price = entity.Price,
            CurrencyId = entity.CurrencyId,
            CurrencyCode = entity.Currency.Code
        };
    }
}
