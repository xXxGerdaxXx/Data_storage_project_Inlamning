using Data_storage_project_library.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Dtos
{
    public class ProjectsDto
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }


        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;  


        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;  


        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; } = null!;  


        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;  
    }
}
