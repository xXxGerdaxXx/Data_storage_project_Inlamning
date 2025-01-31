using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Dtos;

public class StatusRegistrationForm
{
    [Required]
    [StringLength(50, ErrorMessage = "Status name cannot exceed 50 characters.")]
    public string Name { get; set; } = null!;
}
