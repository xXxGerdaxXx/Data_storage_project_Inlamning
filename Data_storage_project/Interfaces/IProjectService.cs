using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;

namespace Data_storage_project_library.Interfaces;

public interface IProjectService
{
    Task<ProjectEntity?> RegisterProjectAsync(ProjectRegistrationForm form);
    Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync();
    Task<ProjectEntity?> GetProjectByIdAsync(string projectId);
    Task<ProjectEntity?> UpdateProjectAsync(string projectId, ProjectRegistrationForm form);
    Task<bool> DeleteProjectAsync(string projectId);

}

