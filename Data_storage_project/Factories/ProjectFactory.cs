using Data_storage_project_library.Dtos;
using Data_storage_project_library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_storage_project_library.Factories;

public static class ProjectFactory
{
    public static ProjectsDto Create(ProjectEntity entity)
    {
        return new ProjectsDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            CustomerId = entity.CustomerId,
            CustomerName = entity.Customer.CustomerName, 
            StatusId = entity.StatusId,
            StatusName = entity.Status.Name,  
            EmployeeId = entity.EmployeeId,
            EmployeeFullName = $"{entity.Employee.FirstName} {entity.Employee.LastName}",  
            ServiceId = entity.ServiceId,
            ServiceName = entity.Service.ServiceName  
        };
    }
}
