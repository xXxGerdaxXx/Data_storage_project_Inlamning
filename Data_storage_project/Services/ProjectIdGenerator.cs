using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;

namespace Data_storage_project_library.Services;

public class ProjectIdGenerator(IProjectRepository projectRepository) : IProjectIdGenerator
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<string> GenerateProjectIdAsync()
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
