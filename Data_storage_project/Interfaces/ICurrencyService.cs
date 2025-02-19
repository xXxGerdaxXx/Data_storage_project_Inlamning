using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface ICurrencyService
{
    Task<CurrencyDto?> RegisterCurrencyAsync(CurrencyRegistrationForm form);
    Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync();
    Task<CurrencyDto?> GetCurrencyByIdAsync(int currencyId);
    Task<CurrencyDto?> UpdateCurrencyAsync(int currencyId, CurrencyRegistrationForm form);
    Task<bool> DeleteCurrencyAsync(int currencyId);
}
