using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Dtos;

public class CustomerUpdateForm
{
    [Required]
    [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters.")]
    public string CustomerName { get; set; } = null!;

    public List<CustomerContactDto> CustomerContacts { get; set; } = new();
}
