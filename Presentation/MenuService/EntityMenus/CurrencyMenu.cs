using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace Presentation.MenuService.EntityMenus;

public class CurrencyMenu(ICurrencyService currencyService)
{
    private readonly ICurrencyService _currencyService = currencyService;

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Currency Management ===");
            Console.WriteLine("1. View All Currencies");
            Console.WriteLine("2. Add New Currency");
            Console.WriteLine("3. Update Currency");
            Console.WriteLine("4. Delete Currency");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ViewAllCurrenciesAsync();
                    break;
                case "2":
                    await AddNewCurrencyAsync();
                    break;
                case "3":
                    await UpdateCurrencyAsync();
                    break;
                case "4":
                    await DeleteCurrencyAsync();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ViewAllCurrenciesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Currencies ===");

        var currencies = await _currencyService.GetAllCurrenciesAsync();
        if (currencies == null || !currencies.Any()) 
        {
            Console.WriteLine("No currencies found.");
        }
        else
        {
            foreach (var currency in currencies)
            {
                Console.WriteLine($"ID: {currency?.Id}, Code: {currency?.Code}, Name: {currency?.Name }");
            }
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private async Task AddNewCurrencyAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Currency ===");

        Console.Write("Enter Currency Code (e.g., USD, EUR): ");
        var currencyCode = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            Console.WriteLine("Error: Currency code is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Currency Name: ");
        var currencyName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(currencyName))
        {
            Console.WriteLine("Error: Currency name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new CurrencyRegistrationForm
        {
            Code = currencyCode,
            Name = currencyName
        };

        var currency = await _currencyService.RegisterCurrencyAsync(form);
        Console.WriteLine(currency != null ? "Currency added successfully!" : "Error adding currency.");
        Console.ReadKey();
    }

    private async Task UpdateCurrencyAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Currency ===");

        var currencies = await _currencyService.GetAllCurrenciesAsync();
        if (!currencies.Any())
        {
            Console.WriteLine("No currencies found.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nAvailable Currencies:");
        foreach (var currency in currencies)
        {
            Console.WriteLine($"ID: {currency.Id}, Code: {currency.Code}, Name: {currency.Name}");
        }

        Console.Write("\nEnter Currency ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int currencyId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Fetch existing currency for preview
        var existingCurrency = await _currencyService.GetCurrencyByIdAsync(currencyId);
        if (existingCurrency == null)
        {
            Console.WriteLine($"Currency with ID {currencyId} not found. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write($"Enter New Currency Code (Current: {existingCurrency.Code}): ");
        var currencyCode = Console.ReadLine()?.Trim();
        currencyCode = string.IsNullOrWhiteSpace(currencyCode) ? existingCurrency.Code : currencyCode;

        Console.Write($"Enter New Currency Name (Current: {existingCurrency.Name}): ");
        var currencyName = Console.ReadLine()?.Trim();
        currencyName = string.IsNullOrWhiteSpace(currencyName) ? existingCurrency.Name : currencyName;

        var form = new CurrencyRegistrationForm
        {
            Code = currencyCode,
            Name = currencyName
        };

        var updatedCurrency = await _currencyService.UpdateCurrencyAsync(currencyId, form);
        Console.WriteLine(updatedCurrency != null ? "Currency updated successfully!" : "Error updating currency.");
        Console.ReadKey();
    }


    private async Task DeleteCurrencyAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Currency ===");

        var currencies = await _currencyService.GetAllCurrenciesAsync();
        if (!currencies.Any())
        {
            Console.WriteLine("No currencies found.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nAvailable Currencies:");
        foreach (var currency in currencies)
        {
            Console.WriteLine($"ID: {currency.Id}, Code: {currency.Code}, Name: {currency.Name}");
        }

        Console.Write("\nEnter Currency ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int currencyId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Check if the currency exists
        var currencyToDelete = await _currencyService.GetCurrencyByIdAsync(currencyId);
        if (currencyToDelete == null)
        {
            Console.WriteLine($"Currency with ID {currencyId} not found. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Attempt to delete the currency
        var success = await _currencyService.DeleteCurrencyAsync(currencyId);
        Console.WriteLine(success
            ? "Currency deleted successfully!"
            : "Error deleting currency. It may be referenced by other entities.");
        Console.ReadKey();
    }


}
