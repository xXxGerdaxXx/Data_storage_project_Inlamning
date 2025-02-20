using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface ICurrencyRepository : IBaseRepository<CurrencyEntity>
{
    Task<CurrencyEntity?> GetByCodeAsync(string code);
}
