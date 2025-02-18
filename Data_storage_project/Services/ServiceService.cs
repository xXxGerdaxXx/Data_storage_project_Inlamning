using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Services;

public class ServiceService(IBaseRepository<ServiceEntity> _serviceRepository, ApplicationDbContext _context) : IServiceService
{
    public async Task<ServiceDto?> RegisterServiceAsync(ServiceRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Service registration form cannot be null.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var newService = new ServiceEntity
            {
                ServiceName = form.ServiceName,
                Price = form.Price,
                CurrencyId = form.CurrencyId
            };

            await _serviceRepository.CreateAsync(newService); // ✅ Use repository method

            await transaction.CommitAsync();
            return ServiceFactory.Create(newService); // ✅ Fixed variable name
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
    {
        var services = await _context.Services
            .Include(s => s.Currency) // ✅ Ensure Currency is included
            .ToListAsync();

        return services.Select(ServiceFactory.Create).ToList();
    }

    public async Task<ServiceDto?> GetServiceByIdAsync(int serviceId)
    {
        var service = await _context.Services
            .Include(s => s.Currency) // ✅ Ensure Currency is included
            .FirstOrDefaultAsync(s => s.Id == serviceId);

        return service != null ? ServiceFactory.Create(service) : null;
    }

    public async Task<ServiceDto?> UpdateServiceAsync(int serviceId, ServiceRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Service registration form cannot be null.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingService = await _serviceRepository.GetAsync(s => s.Id == serviceId)
                ?? throw new KeyNotFoundException($"Service with ID {serviceId} not found.");

            existingService.ServiceName = form.ServiceName;
            existingService.Price = form.Price;
            existingService.CurrencyId = form.CurrencyId;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return ServiceFactory.Create(existingService); // ✅ Fixed variable name
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteServiceAsync(int serviceId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var deleted = await _serviceRepository.DeleteAsync(s => s.Id == serviceId); // ✅ Use repository method
            await transaction.CommitAsync();
            return deleted;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
