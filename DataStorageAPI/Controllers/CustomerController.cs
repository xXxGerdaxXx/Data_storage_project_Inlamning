using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace DataStorageAPI.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return NotFound($"Customer with ID {id} not found.");

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> RegisterCustomer([FromBody] CustomerRegistrationForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdCustomer = await _customerService.RegisterCustomerAsync(form);
        if (createdCustomer == null)
            return BadRequest("Failed to create customer.");

        return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, [FromBody] CustomerRegistrationForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedCustomer = await _customerService.UpdateCustomerAsync(id, form);
        if (updatedCustomer == null)
            return NotFound($"Customer with ID {id} not found.");

        return Ok(updatedCustomer);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        var isDeleted = await _customerService.DeleteCustomerAsync(id);
        if (!isDeleted)
            return NotFound($"Customer with ID {id} not found.");

        return NoContent();
    }

}
