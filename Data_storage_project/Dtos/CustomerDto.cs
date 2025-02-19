using Data_storage_project_library.Dtos;

public class CustomerDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = null!;

    public ICollection<CustomerContactDto>? CustomerContact { get; set; }
}
