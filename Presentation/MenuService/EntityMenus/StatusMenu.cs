using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace Presentation.MenuService.EntityMenus;

public class StatusMenu(IStatusService statusService)
{
    private readonly IStatusService _statusService = statusService;

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Status Management ===");
            Console.WriteLine("1. View All Statuses");
            Console.WriteLine("2. Add New Status");
            Console.WriteLine("3. Update Status");
            Console.WriteLine("4. Delete Status");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ViewAllStatusesAsync();
                    break;
                case "2":
                    await AddNewStatusAsync();
                    break;
                case "3":
                    await UpdateStatusAsync();
                    break;
                case "4":
                    await DeleteStatusAsync();
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

    private async Task ViewAllStatusesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Statuses ===");

        var statuses = await _statusService.GetAllStatusesAsync();
        foreach (var status in statuses)
        {
            Console.WriteLine($"ID: {status.Id}, Name: {status.Name}"); 
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private async Task AddNewStatusAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Status ===");

        Console.Write("Enter Status Name: ");
        var statusName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(statusName))
        {
            Console.WriteLine("Error: Status name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new StatusRegistrationForm
        {
            Name = statusName 
        };

        var status = await _statusService.RegisterStatusAsync(form);
        Console.WriteLine(status != null ? "Status added successfully!" : "Error adding status.");
        Console.ReadKey();
    }

    private async Task UpdateStatusAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Status ===");

        Console.Write("Enter Status ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int statusId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Status Name: ");
        var statusName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(statusName))
        {
            Console.WriteLine("Error: Status name is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new StatusRegistrationForm
        {
            Name = statusName 
        };

        var updatedStatus = await _statusService.UpdateStatusAsync(statusId, form);
        Console.WriteLine(updatedStatus != null ? "Status updated successfully!" : "Error updating status.");
        Console.ReadKey();
    }

    private async Task DeleteStatusAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Status ===");

        Console.Write("Enter Status ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int statusId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var success = await _statusService.DeleteStatusAsync(statusId);
        Console.WriteLine(success ? "Status deleted successfully!" : "Status not found.");
        Console.ReadKey();
    }
}
