using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Dtos;

public class ServiceDto
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = null!;
    public decimal Price { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyCode { get; set; } = null!;
    public string FormattedPrice { get; set; } = null!;
}

