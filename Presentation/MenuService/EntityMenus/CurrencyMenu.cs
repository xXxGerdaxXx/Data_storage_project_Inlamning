using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace Presentation.MenuService.EntityMenus;

public class CurrencyMenu
{
    private readonly ICurrencyService _currencyService;

    public CurrencyMenu(ICurrencyService currencyService)
    {
        _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService)); 
    }

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
                Console.WriteLine($"ID: {currency?.Id}, Code: {currency?.Code ?? "Unknown"}, Name: {currency?.Name ?? "Unknown"}");
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

        Console.Write("Enter Currency ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int currencyId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Currency Code: ");
        var currencyCode = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            Console.WriteLine("Error: Currency code is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Currency Name: ");
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

        var updatedCurrency = await _currencyService.UpdateCurrencyAsync(currencyId, form);
        Console.WriteLine(updatedCurrency != null ? "Currency updated successfully!" : "Error updating currency.");
        Console.ReadKey();
    }

    private async Task DeleteCurrencyAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Currency ===");

        Console.Write("Enter Currency ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int currencyId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var success = await _currencyService.DeleteCurrencyAsync(currencyId);
        Console.WriteLine(success ? "Currency deleted successfully!" : "Currency not found.");
        Console.ReadKey();
    }
}
