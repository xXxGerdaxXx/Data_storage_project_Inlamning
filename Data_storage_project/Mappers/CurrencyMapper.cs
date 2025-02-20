using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Mappers;

public static class CurrencyMapper
{
    public static CurrencyDto ToDto(CurrencyEntity entity)
    {
        return new CurrencyDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name
        };
    }
}
