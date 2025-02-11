using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;

namespace Data_storage_project_library.Services;

public class CurrencyService(IBaseRepository<CurrencyEntity> currencyRepository, ApplicationDbContext context) : ICurrencyService
{
    private readonly IBaseRepository<CurrencyEntity> _currencyRepository = currencyRepository;
    private readonly ApplicationDbContext _context = context;

    public async Task<CurrencyEntity?> RegisterCurrencyAsync(CurrencyRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Currency registration form cannot be null.");

        using (var transaction = await _context.Database.BeginTransactionAsync()) 
        {
            try
            {
                var normalizedCode = form.Code.ToUpper();

                var existingCurrency = await _currencyRepository.GetAsync(c => c.Code == normalizedCode);
                if (existingCurrency != null)
                    throw new ArgumentException($"Currency with code '{normalizedCode}' already exists.");

                var currency = CurrencyRegistrationFactory.CreateCurrency(new CurrencyRegistrationForm
                {
                    Code = normalizedCode,
                    Name = form.Name
                });

                _context.Currencies.Add(currency); 
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return currency;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }
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

        using (var transaction = await _context.Database.BeginTransactionAsync()) 
        {
            try
            {
                var existingCurrency = await _currencyRepository.GetAsync(c => c.Id == currencyId);
                if (existingCurrency == null)
                    throw new KeyNotFoundException($"Currency with ID {currencyId} not found.");

                var normalizedCode = form.Code.ToUpper();

                var duplicateCurrency = await _currencyRepository.GetAsync(c => c.Code == normalizedCode && c.Id != currencyId);
                if (duplicateCurrency != null)
                    throw new ArgumentException($"Another currency with code '{normalizedCode}' already exists.");

                existingCurrency.Code = normalizedCode;
                existingCurrency.Name = form.Name;

                await _context.SaveChangesAsync(); 
                await transaction.CommitAsync();
                return existingCurrency;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }
    }

    public async Task<bool> DeleteCurrencyAsync(int currencyId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync()) 
        {
            try
            {
                var currency = await _currencyRepository.GetAsync(c => c.Id == currencyId);
                if (currency == null)
                    throw new KeyNotFoundException($"Currency with ID {currencyId} not found.");

                _context.Currencies.Remove(currency); 
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }
    }
}
