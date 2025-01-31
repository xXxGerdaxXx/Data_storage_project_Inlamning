using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;


namespace Data_storage_project_library.Factories;

public static class ProjectRegistrationFactory
{
    public static ProjectEntity CreateProject(ProjectRegistrationForm form, string generatedId)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Project registration form cannot be null.");

        return new ProjectEntity
        {
            Id = generatedId, // Assigns the ID provided by the service
            Title = form.Title,
            Description = form.Description,
            StartDate = form.StartDate,
            EndDate = form.EndDate,
            CustomerId = form.CustomerId,
            StatusId = form.StatusId,
            EmployeeId = form.EmployeeId,
            ServiceId = form.ServiceId
        };
    }
}
