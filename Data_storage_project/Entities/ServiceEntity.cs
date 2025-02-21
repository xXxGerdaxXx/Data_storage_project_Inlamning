using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data_storage_project_library.Entities;

public class ServiceEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
    public string ServiceName { get; set; } = null!;
    /// <summary>
    /// I accidently wrote wrong precission and now i get 4 zeros after a comma when 
    /// the price is displayed.
    /// </summary>
    [Required]
    [Precision(18, 4)]  
    [Range(0.01, 9999999.99, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required]
    [ForeignKey("Currency")] 
    public int CurrencyId { get; set; }

    public virtual CurrencyEntity Currency { get; set; } = null!;
}

