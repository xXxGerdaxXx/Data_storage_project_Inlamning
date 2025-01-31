using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Entities;

public class RoleEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string RoleName { get; set; } = null!;

    
    public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
}
