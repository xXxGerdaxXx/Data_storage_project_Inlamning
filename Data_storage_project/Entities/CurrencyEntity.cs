using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Entities;

public class CurrencyEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(3, ErrorMessage = "Currency code must be exactly 3 characters.")]
    public string Code { get; set; } = null!; // Example: USD, EUR, GBP

    [Required]
    [StringLength(50, ErrorMessage = "Currency name cannot exceed 50 characters.")]
    public string Name { get; set; } = null!; // Example: US Dollar, Euro

    // Navigation 
    public ICollection<ServiceEntity> Services { get; set; } = [];
}
