using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Dtos;

public class CurrencyRegistrationForm
{
    [Required]
    [StringLength(3, ErrorMessage = "Currency code must be exactly 3 characters.")]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(50, ErrorMessage = "Currency name cannot exceed 50 characters.")]
    public string Name { get; set; } = null!;
}
