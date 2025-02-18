using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IProjectService
{
    Task<ProjectsDto?> RegisterProjectAsync(ProjectRegistrationForm form);
    Task<IEnumerable<ProjectsDto>> GetAllProjectsAsync();
    Task<ProjectsDto?> GetProjectByIdAsync(string projectId);
    Task<ProjectsDto?> UpdateProjectAsync(string projectId, ProjectRegistrationForm form);
    Task<bool> DeleteProjectAsync(string projectId);
}
