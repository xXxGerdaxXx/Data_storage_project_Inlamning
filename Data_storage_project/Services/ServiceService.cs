using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Services;

public class ServiceService(IBaseRepository<ServiceEntity> serviceRepository, ApplicationDbContext context) : IServiceService
{
    private readonly IBaseRepository<ServiceEntity> _serviceRepository = serviceRepository;
    private readonly ApplicationDbContext _context = context;

    public async Task<ServiceEntity?> RegisterServiceAsync(ServiceRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Service registration form cannot be null.");

        using (var transaction = await _context.Database.BeginTransactionAsync()) 
        {
            try
            {
                var newService = new ServiceEntity
                {
                    ServiceName = form.ServiceName,
                    Price = form.Price,
                    CurrencyId = form.CurrencyId
                };

                _context.Services.Add(newService); 
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return newService;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }
    }

    public async Task<IEnumerable<ServiceEntity>> GetAllServicesAsync()
    {
        return await _serviceRepository.GetAllAsync(s => s.Currency); 
    }

    public async Task<ServiceEntity?> GetServiceByIdAsync(int serviceId)
    {
        return await _serviceRepository.GetAsync(s => s.Id == serviceId, s => s.Currency); 
    }

    public async Task<ServiceEntity?> UpdateServiceAsync(int serviceId, ServiceRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Service registration form cannot be null.");

        using (var transaction = await _context.Database.BeginTransactionAsync()) 
        {
            try
            {
                var existingService = await _serviceRepository.GetAsync(s => s.Id == serviceId)
                    ?? throw new KeyNotFoundException($"Service with ID {serviceId} not found.");

                existingService.ServiceName = form.ServiceName;
                existingService.Price = form.Price;
                existingService.CurrencyId = form.CurrencyId;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return existingService;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }
    }

    public async Task<bool> DeleteServiceAsync(int serviceId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync()) 
        {
            try
            {
                var service = await _serviceRepository.GetAsync(s => s.Id == serviceId);
                if (service == null)
                    throw new KeyNotFoundException($"Service with ID {serviceId} not found.");

                _context.Services.Remove(service); 
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
}
