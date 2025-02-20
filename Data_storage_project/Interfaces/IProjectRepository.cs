using Data_storage_project_library.Entities;
using System.Linq.Expressions;

namespace Data_storage_project_library.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    Task<ProjectEntity?> GetByIdAsync(string projectId);
    Task<ProjectEntity?> GetLastProjectAsync();
}
