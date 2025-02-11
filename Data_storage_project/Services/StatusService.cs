using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;

namespace Data_storage_project_library.Services;

public class StatusService(IBaseRepository<StatusTypeEntity> statusRepository, ApplicationDbContext context) : IStatusService
{
    private readonly IBaseRepository<StatusTypeEntity> _statusRepository = statusRepository;
    private readonly ApplicationDbContext _context = context;


    public async Task<StatusTypeEntity?> RegisterStatusAsync(StatusRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Status registration form cannot be null.");

        using (var transaction = await _context.Database.BeginTransactionAsync()) 
        {
            try
            {
                var existingStatus = await _statusRepository.GetAsync(s => s.Name == form.Name);
                if (existingStatus != null)
                    throw new ArgumentException("Status name already exists.");

                var status = StatusRegistrationFactory.CreateStatus(form);

                _context.Statuses.Add(status); 
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return status;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
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
        using (var transaction = await _context.Database.BeginTransactionAsync()) 
        {
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
    }

    public async Task<StatusTypeEntity?> UpdateStatusAsync(int statusId, StatusRegistrationForm form)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync()) 
        {
            try
            {
                var existingStatus = await _statusRepository.GetAsync(s => s.Id == statusId)
                    ?? throw new KeyNotFoundException($"Status with ID {statusId} not found.");

                existingStatus.Name = form.Name;

                await _context.SaveChangesAsync(); 
                await transaction.CommitAsync();
                return existingStatus;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }
    }
}
