using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_storage_project_library.Services;

public class StatusService(IBaseRepository<StatusTypeEntity> statusRepository, ApplicationDbContext context) : IStatusService
{
    private readonly IBaseRepository<StatusTypeEntity> _statusRepository = statusRepository;
    private readonly ApplicationDbContext _context = context;

    public async Task<StatusTypeDto?> RegisterStatusAsync(StatusRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Status registration form cannot be null.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingStatus = await _statusRepository.GetAsync(s => s.Name == form.Name);
            if (existingStatus != null)
                throw new ArgumentException("Status name already exists.");

            var status = StatusTypeFactory.CreateStatusType(form.Name);

            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return ConvertToDto(status);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<StatusTypeDto>> GetAllStatusesAsync()
    {
        var statuses = await _statusRepository.GetAllAsync();
        return statuses.Select(ConvertToDto);
    }

    public async Task<StatusTypeDto?> GetStatusByIdAsync(int statusId)
    {
        var status = await _statusRepository.GetAsync(s => s.Id == statusId);
        if (status == null)
            throw new KeyNotFoundException($"Status with ID {statusId} not found.");

        return ConvertToDto(status);
    }

    public async Task<bool> DeleteStatusAsync(int statusId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var status = await _statusRepository.GetAsync(s => s.Id == statusId);
            if (status == null)
                throw new KeyNotFoundException($"Status with ID {statusId} not found.");

            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<StatusTypeDto?> UpdateStatusAsync(int statusId, StatusRegistrationForm form)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingStatus = await _statusRepository.GetAsync(s => s.Id == statusId)
                ?? throw new KeyNotFoundException($"Status with ID {statusId} not found.");

            existingStatus.Name = form.Name;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return ConvertToDto(existingStatus);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Converts a StatusTypeEntity to StatusDto.
    /// </summary>
    private static StatusTypeDto ConvertToDto(StatusTypeEntity entity)
    {
        return new StatusTypeDto
        {
            Id = entity.Id,
            Name = entity.Name,
            IsCompleted = entity.IsCompleted
        };
    }
}
