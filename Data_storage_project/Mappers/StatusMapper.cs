using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Mappers;

public static class StatusMapper
{
    public static StatusTypeDto ToDto(StatusTypeEntity entity)
    {
        return new StatusTypeDto
        {
            Id = entity.Id,
            Name = entity.Name,
            IsCompleted = entity.IsCompleted
        };
    }
}

