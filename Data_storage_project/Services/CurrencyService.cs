using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Mappers;
using Data_storage_project_library.Dtos;

namespace Data_storage_project_library.Services;

public class CurrencyService(ICurrencyRepository currencyRepository, IUnitOfWork unitOfWork, ILoggerService logger) : ICurrencyService
{
    private readonly ICurrencyRepository _currencyRepository = currencyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILoggerService _logger = logger;  

    public async Task<CurrencyDto?> RegisterCurrencyAsync(CurrencyRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Currency registration form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            try
            {
                var normalizedCode = form.Code.ToUpper();
                var existingCurrency = await _currencyRepository.GetByCodeAsync(normalizedCode);
                if (existingCurrency != null)
                {
                    _logger.LogWarning($"Currency with code '{normalizedCode}' already exists.");
                    throw new ArgumentException($"Currency with code '{normalizedCode}' already exists.");
                }

                var currency = CurrencyRegistrationFactory.CreateCurrency(form);
                var createdCurrency = await _currencyRepository.CreateAsync(currency);

                _logger.LogInformation($"New currency registered: {createdCurrency!.Code}");

                return createdCurrency != null ? CurrencyMapper.ToDto(createdCurrency) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering currency: {ex.Message}", ex);
                throw;
            }
        });
    }

    public async Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync()
    {
        var currencies = await _currencyRepository.GetAllAsync() ?? [];
        return currencies.Select(CurrencyMapper.ToDto);
    }

    public async Task<CurrencyDto?> GetCurrencyByIdAsync(int currencyId)
    {
        var currency = await _currencyRepository.GetAsync(c => c.Id == currencyId);
        if (currency == null)
            throw new KeyNotFoundException($"Currency with ID {currencyId} not found.");

        return CurrencyMapper.ToDto(currency);
    }

    public async Task<CurrencyDto?> UpdateCurrencyAsync(int currencyId, CurrencyRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Currency registration form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var existingCurrency = await _currencyRepository.GetAsync(c => c.Id == currencyId)
                ?? throw new KeyNotFoundException($"Currency with ID {currencyId} not found.");

            var normalizedCode = form.Code.ToUpper();
            var duplicateCurrency = await _currencyRepository.GetByCodeAsync(normalizedCode);
            if (duplicateCurrency is not null && duplicateCurrency.Id != currencyId)
                throw new ArgumentException($"Another currency with code '{normalizedCode}' already exists.");

            existingCurrency.Code = normalizedCode;
            existingCurrency.Name = form.Name;

            var updatedCurrency = await _currencyRepository.UpdateAsync(existingCurrency, c => c.Id == currencyId);
            return updatedCurrency != null ? CurrencyMapper.ToDto(updatedCurrency) : null;
        });
    }

    public async Task<bool> DeleteCurrencyAsync(int currencyId)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var currency = await _currencyRepository.GetAsync(c => c.Id == currencyId);
            if (currency == null)
            {
                _logger.LogWarning($"Currency with ID {currencyId} not found.");
                return false;
            }

            var isDeleted = await _currencyRepository.DeleteAsync(c => c.Id == currencyId);

            if (isDeleted)
                _logger.LogInformation($"Currency with ID {currencyId} deleted successfully.");
            else
                _logger.LogWarning($"Failed to delete currency with ID {currencyId}.");

            return isDeleted;
        });
    }
}