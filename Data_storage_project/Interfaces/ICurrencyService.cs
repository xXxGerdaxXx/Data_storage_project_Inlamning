using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface ICurrencyService
{
    Task<CurrencyEntity?> RegisterCurrencyAsync(CurrencyRegistrationForm form);
    Task<IEnumerable<CurrencyEntity>> GetAllCurrenciesAsync();
    Task<CurrencyEntity?> GetCurrencyByIdAsync(int currencyId);
    Task<CurrencyEntity?> UpdateCurrencyAsync(int currencyId, CurrencyRegistrationForm form);
    Task<bool> DeleteCurrencyAsync(int currencyId);
}
