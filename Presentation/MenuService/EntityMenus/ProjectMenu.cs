using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace Presentation.MenuService.EntityMenus;

public class ProjectMenu
{
    private readonly IProjectService _projectService;

    public ProjectMenu(IProjectService projectService)
    {
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService)); 
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Project Management ===");
            Console.WriteLine("1. View All Projects");
            Console.WriteLine("2. Add New Project");
            Console.WriteLine("3. Update Project");
            Console.WriteLine("4. Delete Project");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ViewAllProjectsAsync();
                    break;
                case "2":
                    await AddNewProjectAsync();
                    break;
                case "3":
                    await UpdateProjectAsync();
                    break;
                case "4":
                    await DeleteProjectAsync();
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

    private async Task ViewAllProjectsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Projects ===");

        var projects = await _projectService.GetAllProjectsAsync();
        if (projects == null || !projects.Any()) 
        {
            Console.WriteLine("No projects found.");
        }
        else
        {
            foreach (var project in projects)
            {
                Console.WriteLine($"ID: {project?.Id}, Title: {project?.Title ?? "Unknown"}, Start Date: {project?.StartDate.ToShortDateString()}");
            }
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private async Task AddNewProjectAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Project ===");

        Console.Write("Enter Project Title: ");
        var title = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Error: Project title is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Project Description (optional): ");
        var description = Console.ReadLine()?.Trim();

        Console.Write("Enter Start Date (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
        {
            Console.WriteLine("Invalid date format. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Invalid Customer ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Status ID: ");
        if (!int.TryParse(Console.ReadLine(), out int statusId))
        {
            Console.WriteLine("Invalid Status ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Employee ID: ");
        if (!int.TryParse(Console.ReadLine(), out int employeeId))
        {
            Console.WriteLine("Invalid Employee ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Service ID: ");
        if (!int.TryParse(Console.ReadLine(), out int serviceId))
        {
            Console.WriteLine("Invalid Service ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new ProjectRegistrationForm
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            CustomerId = customerId,
            StatusId = statusId,
            EmployeeId = employeeId,
            ServiceId = serviceId
        };

        var project = await _projectService.RegisterProjectAsync(form);
        Console.WriteLine(project != null ? "Project added successfully!" : "Error adding project.");
        Console.ReadKey();
    }

    private async Task UpdateProjectAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Project ===");

        Console.Write("Enter Project ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int projectId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Project Title: ");
        var title = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Error: Project title is required. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Project Description (optional): ");
        var description = Console.ReadLine()?.Trim();

        Console.Write("Enter New Start Date (yyyy-mm-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
        {
            Console.WriteLine("Invalid date format. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Invalid Customer ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Status ID: ");
        if (!int.TryParse(Console.ReadLine(), out int statusId))
        {
            Console.WriteLine("Invalid Status ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Employee ID: ");
        if (!int.TryParse(Console.ReadLine(), out int employeeId))
        {
            Console.WriteLine("Invalid Employee ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Service ID: ");
        if (!int.TryParse(Console.ReadLine(), out int serviceId))
        {
            Console.WriteLine("Invalid Service ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var form = new ProjectRegistrationForm
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            CustomerId = customerId,
            StatusId = statusId,
            EmployeeId = employeeId,
            ServiceId = serviceId
        };

        var updatedProject = await _projectService.UpdateProjectAsync(projectId, form);
        Console.WriteLine(updatedProject != null ? "Project updated successfully!" : "Error updating project.");
        Console.ReadKey();
    }

    private async Task DeleteProjectAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Project ===");

        Console.Write("Enter Project ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int projectId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        var success = await _projectService.DeleteProjectAsync(projectId);
        Console.WriteLine(success ? "Project deleted successfully!" : "Project not found.");
        Console.ReadKey();
    }
}
