using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;

namespace Data_storage_project_library.Services;

public class CurrencyService(IBaseRepository<CurrencyEntity> currencyRepository) : ICurrencyService
{
    private readonly IBaseRepository<CurrencyEntity> _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));

    public async Task<CurrencyEntity?> RegisterCurrencyAsync(CurrencyRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Currency registration form cannot be null.");

        // Check if currency already exists
        var existingCurrency = await _currencyRepository.GetAsync(c => c.Code == form.Code);
        if (existingCurrency != null)
            throw new ArgumentException("Currency code already exists.");

        var currency = CurrencyRegistrationFactory.CreateCurrency(form);
        return await _currencyRepository.CreateAsync(currency);
    }

    public async Task<IEnumerable<CurrencyEntity>> GetAllCurrenciesAsync()
    {
        return await _currencyRepository.GetAllAsync();
    }

    public async Task<CurrencyEntity?> GetCurrencyByIdAsync(int currencyId)
    {
        return await _currencyRepository.GetAsync(c => c.Id == currencyId);
    }

    public async Task<CurrencyEntity?> UpdateCurrencyAsync(int currencyId, CurrencyRegistrationForm form)
    {
        var existingCurrency = await _currencyRepository.GetAsync(c => c.Id == currencyId);
        if (existingCurrency == null)
            throw new KeyNotFoundException($"Currency with ID {currencyId} not found.");

        existingCurrency.Code = form.Code;
        existingCurrency.Name = form.Name;

        return await _currencyRepository.UpdateAsync(existingCurrency, c => c.Id == currencyId);
    }


    public async Task<bool> DeleteCurrencyAsync(int currencyId)
    {
        return await _currencyRepository.DeleteAsync(c => c.Id == currencyId);
    }
}
