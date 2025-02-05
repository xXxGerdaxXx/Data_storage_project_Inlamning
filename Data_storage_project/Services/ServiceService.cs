using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;

namespace Data_storage_project_library.Services;

public class ServiceService(IBaseRepository<ServiceEntity> serviceRepository) : IServiceService
{
    private readonly IBaseRepository<ServiceEntity> _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));

    public async Task<ServiceEntity?> RegisterServiceAsync(ServiceRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Service registration form cannot be null.");

        var newService = new ServiceEntity
        {
            ServiceName = form.ServiceName,
            Price = form.Price,
            CurrencyId = form.CurrencyId
        };

        return await _serviceRepository.CreateAsync(newService);
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
        var existingService = await _serviceRepository.GetAsync(s => s.Id == serviceId) ?? throw new KeyNotFoundException($"Service with ID {serviceId} not found.");
        existingService.ServiceName = form.ServiceName;
        existingService.Price = form.Price;
        existingService.CurrencyId = form.CurrencyId;

        return await _serviceRepository.UpdateAsync(existingService, s => s.Id == serviceId);
    }

    public async Task<bool> DeleteServiceAsync(int serviceId)
    {
        return await _serviceRepository.DeleteAsync(s => s.Id == serviceId);
    }
}
