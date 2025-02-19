using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_storage_project_library.Services;

public class CurrencyService(IBaseRepository<CurrencyEntity> currencyRepository, ApplicationDbContext context) : ICurrencyService
{
    private readonly IBaseRepository<CurrencyEntity> _currencyRepository = currencyRepository;
    private readonly ApplicationDbContext _context = context;

    /// <summary>
    /// Registers a new currency.
    /// </summary>
    public async Task<CurrencyDto?> RegisterCurrencyAsync(CurrencyRegistrationForm form)
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

                return ConvertToDto(currency);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    /// <summary>
    /// Retrieves all currencies.
    /// </summary>
    public async Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync()
    {
        var currencies = await _currencyRepository.GetAllAsync();
        return currencies.Select(ConvertToDto);
    }

    /// <summary>
    /// Retrieves a currency by ID.
    /// </summary>
    public async Task<CurrencyDto?> GetCurrencyByIdAsync(int currencyId)
    {
        var currency = await _currencyRepository.GetAsync(c => c.Id == currencyId);
        if (currency == null)
            throw new KeyNotFoundException($"Currency with ID {currencyId} not found.");

        return ConvertToDto(currency);
    }

    /// <summary>
    /// Updates an existing currency.
    /// </summary>
    public async Task<CurrencyDto?> UpdateCurrencyAsync(int currencyId, CurrencyRegistrationForm form)
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

                return ConvertToDto(existingCurrency);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    /// <summary>
    /// Deletes a currency by ID.
    /// </summary>
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

    /// <summary>
    /// Converts a CurrencyEntity to CurrencyDto.
    /// </summary>
    private static CurrencyDto ConvertToDto(CurrencyEntity entity)
    {
        return new CurrencyDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name
        };
    }
}
