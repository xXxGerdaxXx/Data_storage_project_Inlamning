using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Entities;

public class ProjectEntity
{
    [Key]
    [Required]
    [StringLength(5)] 
    public string Id { get; set; } = null!; 

    [Required]
    [StringLength(200, ErrorMessage = "Project title cannot exceed 200 characters.")]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public CustomerEntity Customer { get; set; } = null!;

    [Required]
    public int StatusId { get; set; }

    [ForeignKey(nameof(StatusId))]
    public StatusTypeEntity Status { get; set; } = null!;

    [Required]
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public EmployeeEntity Employee { get; set; } = null!;

    [Required]
    public int ServiceId { get; set; }

    [ForeignKey(nameof(ServiceId))]
    public ServiceEntity Service { get; set; } = null!;
}
