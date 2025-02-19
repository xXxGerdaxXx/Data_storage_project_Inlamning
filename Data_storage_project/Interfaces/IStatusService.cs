using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IStatusService
{
    Task<StatusTypeDto?> RegisterStatusAsync(StatusRegistrationForm form);
    Task<IEnumerable<StatusTypeDto>> GetAllStatusesAsync();
    Task<StatusTypeDto?> GetStatusByIdAsync(int statusId);
    Task<StatusTypeDto?> UpdateStatusAsync(int statusId, StatusRegistrationForm form);
    Task<bool> DeleteStatusAsync(int statusId);
}

