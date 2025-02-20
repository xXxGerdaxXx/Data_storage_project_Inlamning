using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Mappers;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
namespace Data_storage_project_library.Services;

public class ServiceService(IServiceRepository serviceRepository, IUnitOfWork unitOfWork) : IServiceService
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ServiceDto?> RegisterServiceAsync(ServiceRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Service registration form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var newService = ServiceFactory.CreateService(form); 
            var createdService = await _serviceRepository.CreateAsync(newService);
            return createdService != null ? ServiceMapper.ToDto(createdService) : null;
        });
    }

    public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
    {
        var services = await _serviceRepository.GetAllWithCurrencyAsync();
        return services.Select(ServiceMapper.ToDto);
    }

    public async Task<ServiceDto?> GetServiceByIdAsync(int serviceId)
    {
        var service = await _serviceRepository.GetByIdWithCurrencyAsync(serviceId);
        return service != null ? ServiceMapper.ToDto(service) : null;
    }

    public async Task<ServiceDto?> UpdateServiceAsync(int serviceId, ServiceRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Service registration form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var existingService = await _serviceRepository.GetAsync(s => s.Id == serviceId)
                ?? throw new KeyNotFoundException($"Service with ID {serviceId} not found.");

            existingService.ServiceName = form.ServiceName;
            existingService.Price = form.Price;
            existingService.CurrencyId = form.CurrencyId;

            var updatedService = await _serviceRepository.UpdateAsync(existingService, s => s.Id == serviceId);
            return updatedService != null ? ServiceMapper.ToDto(updatedService) : null;
        });
    }

    public async Task<bool> DeleteServiceAsync(int serviceId)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            return await _serviceRepository.DeleteAsync(s => s.Id == serviceId);
        });
    }
}
