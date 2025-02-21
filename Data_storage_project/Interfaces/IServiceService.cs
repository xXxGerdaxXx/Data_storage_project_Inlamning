using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IServiceService
{
    Task<ServiceDto?> RegisterServiceAsync(ServiceRegistrationForm form); 
    Task<IEnumerable<ServiceDto>> GetAllServicesAsync(); 
    Task<ServiceDto?> GetServiceByIdAsync(int serviceId); 
    Task<ServiceDto?> UpdateServiceAsync(int serviceId, ServiceRegistrationForm form); 
    Task<bool> DeleteServiceAsync(int serviceId);

    Task<bool> ExistsAsync(int serviceId);  
}
