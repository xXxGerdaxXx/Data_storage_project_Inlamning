using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;

namespace Data_storage_project_library.Services;

public class StatusService(IBaseRepository<StatusEntity> statusRepository) : IStatusService
{
    private readonly IBaseRepository<StatusEntity> _statusRepository = statusRepository ?? throw new ArgumentNullException(nameof(statusRepository));

    public async Task<StatusEntity?> RegisterStatusAsync(StatusRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Status registration form cannot be null.");

        var existingStatus = await _statusRepository.GetAsync(s => s.Name == form.Name);
        if (existingStatus != null)
            throw new ArgumentException("Status name already exists.");

        var status = StatusRegistrationFactory.CreateStatus(form);
        return await _statusRepository.CreateAsync(status);
    }

    public async Task<IEnumerable<StatusEntity>> GetAllStatusesAsync()
    {
        return await _statusRepository.GetAllAsync();
    }

    public async Task<StatusEntity?> GetStatusByIdAsync(int statusId)
    {
        return await _statusRepository.GetAsync(s => s.Id == statusId);
    }

    public async Task<bool> DeleteStatusAsync(int statusId)
    {
        return await _statusRepository.DeleteAsync(s => s.Id == statusId);
    }
}
