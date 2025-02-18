using Microsoft.AspNetCore.Mvc;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace DataStorageAPI.Controllers;

[ApiController]
[Route("api/services")]
public class ServiceController(IServiceService _serviceService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAllServices()
    {
        var services = await _serviceService.GetAllServicesAsync();
        if (!services.Any()) return NotFound("No services found.");
        return Ok(services);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDto>> GetServiceById(int id)
    {
        var service = await _serviceService.GetServiceByIdAsync(id);
        if (service == null) return NotFound($"Service with ID {id} not found.");
        return Ok(service);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceDto>> RegisterService([FromBody] ServiceRegistrationForm form)
    {
        if (form == null) return BadRequest("Service registration data is required.");

        var createdService = await _serviceService.RegisterServiceAsync(form);
        if (createdService == null) return BadRequest("Failed to create service.");

        return CreatedAtAction(nameof(GetServiceById), new { id = createdService.Id }, createdService);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceDto>> UpdateService(int id, [FromBody] ServiceRegistrationForm form)
    {
        if (form == null) return BadRequest("Service update data is required.");

        var updatedService = await _serviceService.UpdateServiceAsync(id, form);
        if (updatedService == null) return NotFound($"Service with ID {id} not found.");

        return Ok(updatedService);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteService(int id)
    {
        var deleted = await _serviceService.DeleteServiceAsync(id);
        if (!deleted) return NotFound($"Service with ID {id} not found.");

        return NoContent();
    }
}
