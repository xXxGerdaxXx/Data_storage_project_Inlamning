using Microsoft.AspNetCore.Mvc;
using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;

namespace DataStorageAPI.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    private readonly IEmployeeService _employeeService = employeeService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] EmployeeRegistrationForm form)
    {
        if (form.RoleId == 0)
            return BadRequest("RoleId is required.");

        var created = await _employeeService.RegisterEmployeeAsync(form);
        if (created == null)
            return BadRequest("Employee registration failed.");

        return CreatedAtAction(nameof(GetEmployeeById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, [FromBody] EmployeeRegistrationForm form)
    {
        if (form.RoleId == 0)
            return BadRequest("RoleId is required.");

        var updated = await _employeeService.UpdateEmployeeAsync(form, id);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var deleted = await _employeeService.DeleteEmployeeAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
