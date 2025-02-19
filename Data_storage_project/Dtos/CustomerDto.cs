using Data_storage_project_library.Dtos;

namespace Data_storage_project_library.Dtos;

public class CustomerDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = null!;

    // List for One-to-Many relationship
    public List<CustomerContactDto> CustomerContacts { get; set; } = new();
}
