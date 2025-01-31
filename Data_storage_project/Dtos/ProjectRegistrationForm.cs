using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Dtos;

public class ProjectRegistrationForm
{
    [Required]
    [StringLength(200, ErrorMessage = "Project title cannot exceed 200 characters.")]
    public string Title { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Project description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(ProjectRegistrationForm), nameof(ValidateStartDate))]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    [CustomValidation(typeof(ProjectRegistrationForm), nameof(ValidateEndDate))]
    public DateTime? EndDate { get; set; } // Optional (null means ongoing project)

    [Required(ErrorMessage = "Customer ID is required.")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Status ID is required.")]
    public int StatusId { get; set; }

    [Required(ErrorMessage = "Employee ID is required.")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Service ID is required.")]
    public int ServiceId { get; set; }

    // Validation Method: Ensured StartDate is not in the past
    public static ValidationResult? ValidateStartDate(DateTime startDate, ValidationContext context)
    {
        if (startDate < DateTime.UtcNow.Date)
            return new ValidationResult("Start date cannot be in the past.");

        return ValidationResult.Success;
    }

    // Validation Method: Ensure EndDate is after StartDate
    public static ValidationResult? ValidateEndDate(DateTime? endDate, ValidationContext context)
    {
        var instance = (ProjectRegistrationForm)context.ObjectInstance;

        if (endDate.HasValue && endDate.Value < instance.StartDate)
            return new ValidationResult("End date must be after the start date.");

        return ValidationResult.Success;
    }
}
