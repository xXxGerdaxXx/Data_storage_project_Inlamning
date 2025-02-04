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

        // Normalize the code for consistency
        var normalizedCode = form.Code.ToUpper();

        // Check if the currency already exists
        var existingCurrency = await _currencyRepository.GetAsync(c => c.Code == normalizedCode);
        if (existingCurrency != null)
            throw new ArgumentException($"Currency with code '{normalizedCode}' already exists.");

        var currency = CurrencyRegistrationFactory.CreateCurrency(new CurrencyRegistrationForm
        {
            Code = normalizedCode,
            Name = form.Name
        });

        return await _currencyRepository.CreateAsync(currency);
    }


    public async Task<IEnumerable<CurrencyEntity>> GetAllCurrenciesAsync()
    {
        return await _currencyRepository.GetAllAsync();
    }

    public async Task<CurrencyEntity?> GetCurrencyByIdAsync(int currencyId)
    {
        var currency = await _currencyRepository.GetAsync(c => c.Id == currencyId);
        if (currency == null)
            throw new KeyNotFoundException($"Currency with ID {currencyId} not found.");

        return currency;
    }

    public async Task<CurrencyEntity?> UpdateCurrencyAsync(int currencyId, CurrencyRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Currency registration form cannot be null.");

        var existingCurrency = await _currencyRepository.GetAsync(c => c.Id == currencyId);
        if (existingCurrency == null)
            throw new KeyNotFoundException($"Currency with ID {currencyId} not found.");

        // Normalize the code for consistency
        var normalizedCode = form.Code.ToUpper();

        // Check if another currency with the same code already exists
        var duplicateCurrency = await _currencyRepository.GetAsync(c => c.Code == normalizedCode && c.Id != currencyId);
        if (duplicateCurrency != null)
            throw new ArgumentException($"Another currency with code '{normalizedCode}' already exists.");

        existingCurrency.Code = normalizedCode;
        existingCurrency.Name = form.Name;

        return await _currencyRepository.UpdateAsync(existingCurrency, c => c.Id == currencyId);
    }



    public async Task<bool> DeleteCurrencyAsync(int currencyId)
    {
        // Checking if the currency exists
        var currency = await _currencyRepository.GetAsync(c => c.Id == currencyId);
        if (currency == null)
            throw new KeyNotFoundException($"Currency with ID {currencyId} not found.");

        // Could add a a check for dependencies, need to come back to it

        return await _currencyRepository.DeleteAsync(c => c.Id == currencyId);
    }
}
