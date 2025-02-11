using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;


namespace Data_storage_project_library.Services;

public class ProjectService(ProjectRepository repository, StatusService statusService, ApplicationDbContext context) : IProjectService
{
    private readonly ProjectRepository _projectRepository = repository;
    private readonly StatusService _statusService = statusService;
    private readonly ApplicationDbContext _context = context;


    public async Task<ProjectEntity?> RegisterProjectAsync(ProjectRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Project registration form cannot be null.");

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var nextId = await GenerateProjectIdAsync();

                var project = ProjectRegistrationFactory.CreateProject(form, nextId);
                var createdProject = await _projectRepository.CreateAsync(project);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return createdProject;

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        
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
        using (var transaction =await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (string.IsNullOrWhiteSpace(projectId) || !projectId.StartsWith("P-"))
                    throw new ArgumentException("Invalid project ID format. Expected format: 'P-123'.", nameof(projectId));

                var project = await _projectRepository.GetAsync(p => p.Id == projectId);
                if (project == null)
                    throw new KeyNotFoundException($"Project with ID {projectId} not found.");
                bool isDeleted = await _projectRepository.DeleteAsync(p => p.Id == projectId);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return isDeleted;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }


    public async Task<ProjectEntity?> UpdateProjectAsync(string projectId, ProjectRegistrationForm form)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var existingProject = await _projectRepository.GetAsync(p => p.Id == projectId)
                ?? throw new KeyNotFoundException($"Project with ID {projectId} not found.");

                var status = await _statusService.GetStatusByIdAsync(form.StatusId)
                    ?? throw new KeyNotFoundException($"Status with ID {form.StatusId} not found.");

                if (status.Name == "Completed" && existingProject.EndDate == null)
                {
                    existingProject.EndDate = DateTime.UtcNow;
                }
                existingProject.Title = form.Title;
                existingProject.Description = form.Description;
                existingProject.StartDate = form.StartDate;
                existingProject.CustomerId = form.CustomerId;
                existingProject.StatusId = form.StatusId;
                existingProject.EmployeeId = form.EmployeeId;
                existingProject.ServiceId = form.ServiceId;

                var updatedProject = await _projectRepository.UpdateAsync(existingProject, p => p.Id == projectId);

                await transaction.CommitAsync();
                return updatedProject;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }



 
     //I used chatGPT 4o for this method. It is responsible for generating a unique project ID in the format "P-XXX".
     //The method retrieves the last stored project from the database and extracts the numerical part of its ID.
     //If no projects exist, the first project ID will be "P-1".
     //If parsing fails, an exception is thrown to prevent incorrect ID generation.


    private async Task<string> GenerateProjectIdAsync()
    {
        var lastProject = await _projectRepository.GetLastProjectAsync();

        int lastNumber = 0;
        if (lastProject?.Id != null && lastProject.Id.StartsWith("P-"))
        {
            if (!int.TryParse(lastProject.Id.AsSpan(2), out lastNumber))
            {
                throw new InvalidOperationException("Failed to parse last project ID.");
            }  
        }
        return $"P-{lastNumber + 1}";
    }


}
