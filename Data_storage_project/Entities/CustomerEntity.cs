using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Entities;

public class CustomerEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters.")]
    public string CustomerName { get; set; } = null!;

    public ICollection<CustomerContactEntity> CustomerContacts { get; set; } = [];
}


