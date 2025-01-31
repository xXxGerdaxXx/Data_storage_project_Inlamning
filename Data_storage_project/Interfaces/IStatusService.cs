using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IStatusService
{
    Task<StatusTypeEntity?> RegisterStatusAsync(StatusRegistrationForm form);
    Task<IEnumerable<StatusTypeEntity>> GetAllStatusesAsync();
    Task<StatusTypeEntity?> GetStatusByIdAsync(int statusId);
    Task<StatusTypeEntity?> UpdateStatusAsync(int statusId, StatusRegistrationForm form);
    Task<bool> DeleteStatusAsync(int statusId);
}

