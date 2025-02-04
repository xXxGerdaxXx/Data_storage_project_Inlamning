using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using System.Linq.Expressions;

namespace Data_storage_project_library.Services;

public class ProjectService(ProjectRepository repository, StatusService statusService) : IProjectService
{
    private readonly ProjectRepository _projectRepository = repository;
    private readonly StatusService _statusService = statusService;


    public async Task<ProjectEntity?> RegisterProjectAsync(ProjectRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Project registration form cannot be null.");

        var nextId = await GenerateProjectIdAsync();

        var project = ProjectRegistrationFactory.CreateProject(form, nextId);

        return await _projectRepository.CreateAsync(project);
    }

    public async Task<ProjectEntity?> GetProjectByIdAsync(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId) || !projectId.StartsWith("P-"))
            throw new ArgumentException("Invalid project ID format. Expected format: 'P-123'.", nameof(projectId));

        return await _projectRepository.GetAsync(p => p.Id == projectId);
    }


    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();

        foreach (var project in projects)
        {
            project.Status = await _statusService.GetStatusByIdAsync(project.StatusId);
        }

        return projects;
    }


    public async Task<bool> DeleteProjectAsync(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId) || !projectId.StartsWith("P-"))
            throw new ArgumentException("Invalid project ID format. Expected format: 'P-123'.", nameof(projectId));

        var project = await _projectRepository.GetAsync(p => p.Id == projectId);
        return project == null
            ? throw new KeyNotFoundException($"Project with ID {projectId} not found.")
            : await _projectRepository.DeleteAsync(p => p.Id == projectId);
    }


    public async Task<ProjectEntity?> UpdateProjectAsync(string projectId, ProjectRegistrationForm form)
    {
        var existingProject = await _projectRepository.GetAsync(p => p.Id == projectId)
            ?? throw new KeyNotFoundException($"Project with ID {projectId} not found.");

        // Retrieve status information
        var status = await _statusService.GetStatusByIdAsync(form.StatusId) ?? throw new KeyNotFoundException($"Status with ID {form.StatusId} not found.");

        // Preserve existing EndDate if the project was already completed
        if (status.Name == "Completed" && existingProject.EndDate == null)
        {
            existingProject.EndDate = DateTime.UtcNow; // Set EndDate automatically
        }

        existingProject.Title = form.Title;
        existingProject.Description = form.Description;
        existingProject.StartDate = form.StartDate;
        existingProject.CustomerId = form.CustomerId;
        existingProject.StatusId = form.StatusId;
        existingProject.EmployeeId = form.EmployeeId;
        existingProject.ServiceId = form.ServiceId;

        return await _projectRepository.UpdateAsync(existingProject, p => p.Id == projectId);
    }



    private async Task<string> GenerateProjectIdAsync()
    {
        var lastProject = await _projectRepository.GetLastProjectAsync();

        int lastNumber = 0;
        if (lastProject?.Id != null && lastProject.Id.StartsWith("P-"))
        {
            int.TryParse(lastProject.Id.AsSpan(2), out lastNumber);
        }

        return $"P-{lastNumber + 1}";
    }


}
