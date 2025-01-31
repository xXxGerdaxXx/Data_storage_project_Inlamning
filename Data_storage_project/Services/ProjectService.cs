using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using System.Linq.Expressions;

namespace Data_storage_project_library.Services;

public class ProjectService(ProjectRepository repository) : IProjectService
{
    private readonly ProjectRepository _projectRepository = repository;

    public async Task<ProjectEntity?> RegisterProjectAsync(ProjectRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Project registration form cannot be null.");

        // Generate the next project ID
        var nextId = await GenerateProjectIdAsync();

        // Use the factory to create a new project entity
        var project = ProjectRegistrationFactory.CreateProject(form, nextId);

        // Save to the database via repository
        return await _projectRepository.CreateAsync(project);
    }

    public async Task<ProjectEntity?> GetProjectByIdAsync(int projectId)
    {
        string projectIdString = $"P-{projectId}"; // Convert int to string format "P-101"
        return await _projectRepository.GetAsync(p => p.Id == projectIdString);
    }

    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        return await _projectRepository.GetAllAsync(); // ✅ FIXED: Now included!
    }

    public async Task<bool> DeleteProjectAsync(int projectId)
    {
        string projectIdString = $"P-{projectId}"; // Convert int to string format "P-101"

        var project = await _projectRepository.GetAsync(p => p.Id == projectIdString);
        if (project == null)
            throw new KeyNotFoundException($"Project with ID {projectIdString} not found.");

        return await _projectRepository.DeleteAsync(p => p.Id == projectIdString);
    }

    public async Task<ProjectEntity?> UpdateProjectAsync(int projectId, ProjectRegistrationForm form)
    {
        var existingProject = await _projectRepository.GetAsync(p => p.Id == projectId.ToString());
        if (existingProject == null)
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");

        existingProject.Title = form.Title;
        existingProject.Description = form.Description;
        existingProject.StartDate = form.StartDate;
        existingProject.EndDate = form.EndDate;
        existingProject.CustomerId = form.CustomerId;
        existingProject.StatusId = form.StatusId;
        existingProject.EmployeeId = form.EmployeeId;
        existingProject.ServiceId = form.ServiceId;

        return await _projectRepository.UpdateAsync(existingProject, p => p.Id == projectId.ToString());
    }


    private async Task<string> GenerateProjectIdAsync()
    {
        var lastProject = await _projectRepository.GetLastProjectAsync();

        int lastNumber = 0;
        if (lastProject?.Id != null && lastProject.Id.StartsWith("P-"))
        {
            int.TryParse(lastProject.Id.Substring(2), out lastNumber);
        }

        return $"P-{lastNumber + 1}";
    }


}
