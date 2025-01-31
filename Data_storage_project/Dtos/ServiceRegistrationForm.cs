using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Dtos;

public class ServiceRegistrationForm
{
    [Required]
    [StringLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
    public string ServiceName { get; set; } = null!;

    [Required]
    [Range(0.01, 9999999.99, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required]
    public int CurrencyId { get; set; }
}
