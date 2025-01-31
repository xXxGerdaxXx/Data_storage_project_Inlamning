using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Entities;

public class StatusTypeEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Status name cannot exceed 50 characters.")]
    public string Name { get; set; } = null!; // Example: Pending, Completed, Cancelled
}