using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Dtos;

public class StatusTypeDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Status name cannot exceed 50 characters.")]
    public string Name { get; set; } = null!;
    public bool IsCompleted { get; set; }
}
