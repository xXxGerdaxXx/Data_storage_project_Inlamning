using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Mappers;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Services;

public class StatusService(IStatusRepository statusRepository, IUnitOfWork unitOfWork) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<StatusTypeDto?> RegisterStatusAsync(StatusRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Status registration form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var existingStatus = await _statusRepository.GetByNameAsync(form.Name);
            if (existingStatus != null)
                throw new ArgumentException("Status name already exists.");

            var status = new StatusTypeEntity { Name = form.Name };
            var createdStatus = await _statusRepository.CreateAsync(status);

            return createdStatus != null ? StatusMapper.ToDto(createdStatus) : null;
        });
    }

    public async Task<IEnumerable<StatusTypeDto>> GetAllStatusesAsync()
    {
        var statuses = await _statusRepository.GetAllAsync();
        return statuses.Select(StatusMapper.ToDto);
    }

    public async Task<StatusTypeDto?> GetStatusByIdAsync(int statusId)
    {
        var status = await _statusRepository.GetAsync(s => s.Id == statusId);
        if (status == null)
            throw new KeyNotFoundException($"Status with ID {statusId} not found.");

        return StatusMapper.ToDto(status);
    }

    public async Task<bool> DeleteStatusAsync(int statusId)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var status = await _statusRepository.GetAsync(s => s.Id == statusId);
            if (status == null)
                throw new KeyNotFoundException($"Status with ID {statusId} not found.");

            return await _statusRepository.DeleteAsync(s => s.Id == statusId);
        });
    }

    public async Task<StatusTypeDto?> UpdateStatusAsync(int statusId, StatusRegistrationForm form)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var existingStatus = await _statusRepository.GetAsync(s => s.Id == statusId)
                ?? throw new KeyNotFoundException($"Status with ID {statusId} not found.");

            existingStatus.Name = form.Name;
            var updatedStatus = await _statusRepository.UpdateAsync(existingStatus, s => s.Id == statusId);

            return updatedStatus != null ? StatusMapper.ToDto(updatedStatus) : null;
        });
    }
}
