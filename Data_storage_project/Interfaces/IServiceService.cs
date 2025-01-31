using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IServiceService
{
    Task<ServiceEntity?> RegisterServiceAsync(ServiceRegistrationForm form);
    Task<IEnumerable<ServiceEntity>> GetAllServicesAsync();
    Task<ServiceEntity?> GetServiceByIdAsync(int serviceId);
    Task<ServiceEntity?> UpdateServiceAsync(int serviceId, ServiceRegistrationForm form);
    Task<bool> DeleteServiceAsync(int serviceId);
}
