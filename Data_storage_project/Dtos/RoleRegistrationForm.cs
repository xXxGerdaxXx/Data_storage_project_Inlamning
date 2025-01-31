using System.ComponentModel.DataAnnotations;

public class RoleRegistrationForm
{
    public int? Id { get; set; } // Nullable to differentiate between create & update

    [Required]
    [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters.")]
    public string RoleName { get; set; } = null!;
}
