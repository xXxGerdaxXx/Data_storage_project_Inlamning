using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IProjectService
{
    Task<ProjectEntity?> RegisterProjectAsync(ProjectRegistrationForm form);
    Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync();
    Task<ProjectEntity?> GetProjectByIdAsync(int projectId);
    Task<ProjectEntity?> UpdateProjectAsync(int projectId, ProjectRegistrationForm form);
    Task<bool> DeleteProjectAsync(int projectId);
}

