namespace Data_storage_project_library.Factories;
using Data_storage_project_library.Entities;

public static class StatusTypeFactory
{
    public static StatusTypeEntity CreateStatusType(string statusName)
    {
        return new StatusTypeEntity
        {
            Name = statusName
        };
    }
}
