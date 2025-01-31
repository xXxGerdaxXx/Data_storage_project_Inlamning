using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Data_storage_project_library.Dtos;

public class EmployeeRegistrationForm
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain only letters.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must contain only letters.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Role ID is required.")]
    public int RoleId { get; set; }

    public static ValidationResult? ValidateName(string? name, ValidationContext context)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new ValidationResult($"{context.DisplayName} cannot be empty or whitespace.");

        return ValidationResult.Success;
    }
}
