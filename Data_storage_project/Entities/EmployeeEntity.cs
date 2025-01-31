using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_storage_project_library.Entities;

public class EmployeeEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    
    [Required]
    public int RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public RoleEntity Role { get; set; } = null!; 
}
