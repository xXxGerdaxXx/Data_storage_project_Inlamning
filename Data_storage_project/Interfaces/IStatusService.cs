using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IStatusService
{
    Task<StatusEntity?> RegisterStatusAsync(StatusRegistrationForm form);
    Task<IEnumerable<StatusEntity>> GetAllStatusesAsync();
    Task<StatusEntity?> GetStatusByIdAsync(int statusId);
    Task<bool> DeleteStatusAsync(int statusId);
}
