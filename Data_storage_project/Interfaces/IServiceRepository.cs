using Data_storage_project_library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Interfaces;

public interface IServiceRepository : IBaseRepository<ServiceEntity>
{
    Task<IEnumerable<ServiceEntity>> GetAllWithCurrencyAsync();
    Task<ServiceEntity?> GetByIdWithCurrencyAsync(int serviceId);
}
