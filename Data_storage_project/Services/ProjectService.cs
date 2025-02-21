using Data_storage_project_library.Contexts;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Factories;
using Data_storage_project_library.Interfaces;

namespace Data_storage_project_library.Services;

public class ProjectService(
    IProjectRepository projectRepository,
    ICustomerService customerService,
    IServiceService serviceService,
    IEmployeeService employeeService,
    IStatusService statusService,
    IProjectIdGenerator idGenerator,
    IUnitOfWork unitOfWork) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly ICustomerService _customerService = customerService;
    private readonly IServiceService _serviceService = serviceService;
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly IStatusService _statusService = statusService;
    private readonly IProjectIdGenerator _idGenerator = idGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    // I got this suggestion (and code) for improvement from ChatGPT to create
    // Private validation method ir order to reduce repetition.

    private async Task ValidateProjectDependencies(ProjectRegistrationForm form)
    {
        if (await _customerService.GetCustomerAsync(form.CustomerId) == null)
            throw new ArgumentException($"Customer with ID {form.CustomerId} does not exist.");

        if (!await _serviceService.ExistsAsync(form.ServiceId))
            throw new ArgumentException($"Service with ID {form.ServiceId} does not exist.");

        if (!await _employeeService.ExistsAsync(form.EmployeeId))
            throw new ArgumentException($"Employee with ID {form.EmployeeId} does not exist.");

        if (await _statusService.GetStatusByIdAsync(form.StatusId) == null)
            throw new KeyNotFoundException($"Status with ID {form.StatusId} not found.");
    }

    public async Task<ProjectsDto?> RegisterProjectAsync(ProjectRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Project registration form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            await ValidateProjectDependencies(form); 

            var nextId = await _idGenerator.GenerateProjectIdAsync();
            var project = ProjectRegistrationFactory.CreateProject(form, nextId);
            project.CustomerId = form.CustomerId;

            var createdProject = await _projectRepository.CreateAsync(project);
            return createdProject != null ? ProjectFactory.Create(createdProject) : null;
        });
    }

    public async Task<ProjectsDto?> GetProjectByIdAsync(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId) || !projectId.StartsWith("P-"))
            throw new ArgumentException("Invalid project ID format. Expected format: 'P-123'.", nameof(projectId));

        var project = await _projectRepository.GetAsync(p => p.Id == projectId);
        return project != null ? ProjectFactory.Create(project) : null;
    }

    public async Task<IEnumerable<ProjectsDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return projects.Any() ? projects.Select(ProjectFactory.Create) : new List<ProjectsDto>();
    }

    public async Task<bool> DeleteProjectAsync(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId) || !projectId.StartsWith("P-"))
            throw new ArgumentException("Invalid project ID format. Expected format: 'P-123'.", nameof(projectId));

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var project = await _projectRepository.GetAsync(p => p.Id == projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");

            return await _projectRepository.DeleteAsync(p => p.Id == projectId);
        });
    }

    public async Task<ProjectsDto?> UpdateProjectAsync(string projectId, ProjectRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Project update form cannot be null.");

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var existingProject = await _projectRepository.GetAsync(p => p.Id == projectId)
                ?? throw new KeyNotFoundException($"Project with ID {projectId} not found.");

            await ValidateProjectDependencies(form); 

            var status = await _statusService.GetStatusByIdAsync(form.StatusId);
            if (status!.Name == "Completed" && existingProject.StatusId != form.StatusId && existingProject.EndDate == null)
            {
                existingProject.EndDate = DateTime.UtcNow;
            }

            existingProject.Title = form.Title;
            existingProject.Description = form.Description;
            existingProject.StartDate = form.StartDate;
            existingProject.StatusId = form.StatusId;
            existingProject.EmployeeId = form.EmployeeId;
            existingProject.ServiceId = form.ServiceId;

            var updatedProject = await _projectRepository.UpdateAsync(existingProject, p => p.Id == projectId);
            return updatedProject != null ? ProjectFactory.Create(updatedProject) : null;
        });
    }
}
