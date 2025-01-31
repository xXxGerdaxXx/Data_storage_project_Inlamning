using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;

namespace Data_storage_project_library.Services;

public class StatusService(IBaseRepository<StatusTypeEntity> statusRepository) : IStatusService
{
    private readonly IBaseRepository<StatusTypeEntity> _statusRepository = statusRepository ?? throw new ArgumentNullException(nameof(statusRepository));

    public async Task<StatusTypeEntity?> RegisterStatusAsync(StatusRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Status registration form cannot be null.");

        var existingStatus = await _statusRepository.GetAsync(s => s.Name == form.Name);
        if (existingStatus != null)
            throw new ArgumentException("Status name already exists.");

        var status = StatusRegistrationFactory.CreateStatus(form);
        return await _statusRepository.CreateAsync(status);
    }

    public async Task<IEnumerable<StatusTypeEntity>> GetAllStatusesAsync()
    {
        return await _statusRepository.GetAllAsync();
    }

    public async Task<StatusTypeEntity?> GetStatusByIdAsync(int statusId)
    {
        return await _statusRepository.GetAsync(s => s.Id == statusId);
    }

    public async Task<bool> DeleteStatusAsync(int statusId)
    {
        return await _statusRepository.DeleteAsync(s => s.Id == statusId);
    }

    public async Task<StatusTypeEntity?> UpdateStatusAsync(int statusId, StatusRegistrationForm form)
    {
        var existingStatus = await _statusRepository.GetAsync(s => s.Id == statusId);
        if (existingStatus == null)
            throw new KeyNotFoundException($"Status with ID {statusId} not found.");

        existingStatus.Name = form.Name;

        return await _statusRepository.UpdateAsync(existingStatus, s => s.Id == statusId);
    }

}
