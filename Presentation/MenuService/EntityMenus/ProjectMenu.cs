﻿using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace Presentation.MenuService.EntityMenus;

public class ProjectMenu(
    IProjectService projectService,
    ICustomerService customerService,
    IStatusService statusService,
    IEmployeeService employeeService,
    IServiceService serviceService)
{
    private readonly IProjectService _projectService = projectService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IStatusService _statusService = statusService;
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly IServiceService _serviceService = serviceService;

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

        // Show available customers before selecting
        Console.WriteLine("\n=== Available Customers ===");
        var customers = await _customerService.GetAllCustomersAsync();
        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.Id}, Name: {customer.CustomerName}");
        }
        Console.Write("Select Customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Invalid Customer ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Show available statuses before selecting
        Console.WriteLine("\n=== Project Statuses ===");
        var statuses = await _statusService.GetAllStatusesAsync();
        foreach (var status in statuses)
        {
            Console.WriteLine($"ID: {status.Id}, Name: {status.Name}");
        }
        Console.Write("Select Status ID: ");
        if (!int.TryParse(Console.ReadLine(), out int statusId))
        {
            Console.WriteLine("Invalid Status ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Show available employees before selecting
        Console.WriteLine("\n=== Available Employees ===");
        var employees = await _employeeService.GetAllEmployeesAsync();
        foreach (var employee in employees)
        {
            Console.WriteLine($"ID: {employee.Id}, Name: {employee.FirstName} {employee.LastName}");
        }
        Console.Write("Select Employee ID: ");
        if (!int.TryParse(Console.ReadLine(), out int employeeId))
        {
            Console.WriteLine("Invalid Employee ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Show available services with currency before selecting
        Console.WriteLine("\n=== Available Services ===");
        var services = await _serviceService.GetAllServicesAsync();
        foreach (var service in services)
        {
            Console.WriteLine($"ID: {service.Id}, Name: {service.ServiceName}, Price: {service.Price} {service.Currency}");
        }
        Console.Write("Select Service ID: ");
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

        // Step 1: Prompt user to enter the numeric project ID
        Console.Write("Enter Project ID (numeric only): ");
        if (!int.TryParse(Console.ReadLine(), out int projectId))
        {
            Console.WriteLine("Invalid Project ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Convert numeric ID to "P-123" format
        var projectIdString = $"P-{projectId}";

        // Retrieve the existing project
        var existingProject = await _projectService.GetProjectByIdAsync(projectIdString);
        if (existingProject == null)
        {
            Console.WriteLine("Project not found. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Step 2: Pre-fill data and allow user to modify fields
        Console.Write($"Enter New Project Title (Current: {existingProject.Title}): ");
        var title = Console.ReadLine()?.Trim();
        title = string.IsNullOrWhiteSpace(title) ? existingProject.Title : title;

        Console.Write($"Enter New Project Description (Current: {existingProject.Description ?? "None"}): ");
        var description = Console.ReadLine()?.Trim();
        description = string.IsNullOrWhiteSpace(description) ? existingProject.Description : description;

        Console.Write($"Enter New Start Date (yyyy-mm-dd) (Current: {existingProject.StartDate:yyyy-MM-dd}): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
        {
            startDate = existingProject.StartDate;
        }

        // Step 3: Show and select related entities
        Console.WriteLine("\n=== Available Customers ===");
        var customers = await _customerService.GetAllCustomersAsync();
        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.Id}, Name: {customer.CustomerName}");
        }
        Console.Write($"Select Customer ID (Current: {existingProject.CustomerId}): ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            customerId = existingProject.CustomerId;
        }

        Console.WriteLine("\n=== Project Statuses ===");
        var statuses = await _statusService.GetAllStatusesAsync();
        foreach (var status in statuses)
        {
            Console.WriteLine($"ID: {status.Id}, Name: {status.Name}");
        }
        Console.Write($"Select Status ID (Current: {existingProject.StatusId}): ");
        if (!int.TryParse(Console.ReadLine(), out int statusId))
        {
            statusId = existingProject.StatusId;
        }

        Console.WriteLine("\n=== Available Employees ===");
        var employees = await _employeeService.GetAllEmployeesAsync();
        foreach (var employee in employees)
        {
            Console.WriteLine($"ID: {employee.Id}, Name: {employee.FirstName} {employee.LastName}");
        }
        Console.Write($"Select Employee ID (Current: {existingProject.EmployeeId}): ");
        if (!int.TryParse(Console.ReadLine(), out int employeeId))
        {
            employeeId = existingProject.EmployeeId;
        }

        Console.WriteLine("\n=== Available Services ===");
        var services = await _serviceService.GetAllServicesAsync();
        foreach (var service in services)
        {
            Console.WriteLine($"ID: {service.Id}, Name: {service.ServiceName}, Price: {service.Price} {service.Currency}");
        }
        Console.Write($"Select Service ID (Current: {existingProject.ServiceId}): ");
        if (!int.TryParse(Console.ReadLine(), out int serviceId))
        {
            serviceId = existingProject.ServiceId;
        }

        // Step 4: Create the update form
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

        // Step 5: Update the project
        var updatedProject = await _projectService.UpdateProjectAsync(projectIdString, form);
        Console.WriteLine(updatedProject != null ? "Project updated successfully!" : "Error updating project.");
        Console.ReadKey();
    }




    private async Task DeleteProjectAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Project ===");

        Console.Write("Enter Project ID to delete (numeric only): ");
        if (!int.TryParse(Console.ReadLine(), out int projectId))
        {
            Console.WriteLine("Invalid ID. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Convert integer to string format "P-123"
        var projectIdString = $"P-{projectId}";

        var success = await _projectService.DeleteProjectAsync(projectIdString);
        Console.WriteLine(success ? "Project deleted successfully!" : "Project not found.");
        Console.ReadKey();
    }

}
