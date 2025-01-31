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

    [Required]
    [Precision(18, 4)]  
    [Range(0.01, 9999999.99, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    // Foreign Key to Currency
    [Required]
    public int CurrencyId { get; set; }

    [ForeignKey(nameof(CurrencyId))]
    public CurrencyEntity Currency { get; set; } = null!;
}

