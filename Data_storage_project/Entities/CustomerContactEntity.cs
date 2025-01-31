using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Entities;

public class CustomerContactEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    public string Phone { get; set; } = null!;

    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public CustomerEntity Customer { get; set; } = null!;
}

