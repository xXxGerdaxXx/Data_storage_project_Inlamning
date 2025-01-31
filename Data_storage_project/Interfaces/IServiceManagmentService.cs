using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

public interface IServiceManagementService
{
    Task<ServiceEntity?> CreateServiceAsync(ServiceRegistrationForm form);
    Task<IEnumerable<ServiceEntity>> GetAllServicesAsync();
    Task<ServiceEntity?> GetServiceByIdAsync(int serviceId);
    Task<bool> DeleteServiceAsync(int serviceId);
}
