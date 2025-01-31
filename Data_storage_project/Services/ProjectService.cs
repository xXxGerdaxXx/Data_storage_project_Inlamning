using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using System.Linq.Expressions;

namespace Data_storage_project_library.Services;

public class ProjectService(IBaseRepository<ProjectEntity> projectRepository) : IProjectService
{
    private readonly IBaseRepository<ProjectEntity> _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

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



    private async Task<string> GenerateProjectIdAsync()
    {
        // Fetch the last inserted project using GetLastProjectAsync()
        var lastProject = await _projectRepository.GetLastProjectAsync();

        // Extract the numeric part of the ID
        int lastNumber = 0;
        if (lastProject != null && lastProject.Id.StartsWith("P-"))
        {
            int.TryParse(lastProject.Id.AsSpan(2), out lastNumber);
        }

        // Increment the number and generate the new ID
        return $"P-{lastNumber + 1}";
    }
}
