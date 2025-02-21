using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_storage_project_library.Entities;

//One Employee → Many Projects that i forgot to add 
//public ICollection<ProjectEntity> Projects { get; set; } = [];

//Instead, in my DataContext.cs I have:

//modelBuilder.Entity<ProjectEntity>()
//.HasOne(p => p.Employee)
//.WithMany()
//.HasForeignKey(p => p.EmployeeId)
//.OnDelete(DeleteBehavior.Restrict);

//allowing same employee to be added to multiple projects.
//i understand this is not correct approach but i didn't want 
//to rick and update my entity in case it causes it to crash.

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
