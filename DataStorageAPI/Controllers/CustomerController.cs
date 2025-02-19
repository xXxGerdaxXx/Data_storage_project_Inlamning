using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataStorageAPI.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    /// <summary>
    /// Get all customers with their contact details.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    /// <summary>
    /// Get a single customer by ID, including all their contacts.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return NotFound($"Customer with ID {id} not found.");

        return Ok(customer);
    }

    /// <summary>
    /// Register a new customer with multiple contacts.
    /// </summary>
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

    /// <summary>
    /// Update customer details, including contacts.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, [FromBody] CustomerUpdateForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedCustomer = await _customerService.UpdateCustomerAsync(id, form);
        if (updatedCustomer == null)
            return NotFound($"Customer with ID {id} not found.");

        return Ok(updatedCustomer);
    }

    /// <summary>
    /// Delete a customer and all associated contacts.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        var isDeleted = await _customerService.DeleteCustomerAsync(id);
        if (!isDeleted)
            return NotFound($"Customer with ID {id} not found.");

        return NoContent();
    }

    /// <summary>
    /// Add a new contact to an existing customer.
    /// </summary>
    [HttpPost("{customerId}/contacts")]
    public async Task<ActionResult<CustomerDto>> AddCustomerContact(int customerId, [FromBody] CustomerContactDto contactDto)
    {
        var updatedCustomer = await _customerService.AddCustomerContactAsync(customerId, contactDto);
        if (updatedCustomer == null)
            return NotFound($"Customer with ID {customerId} not found.");

        return Ok(updatedCustomer);
    }


    /// <summary>
    /// Update a specific contact for a customer.
    /// </summary>
    [HttpPut("{customerId}/contacts/{contactId}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomerContact(int customerId, int contactId, [FromBody] CustomerContactDto contactDto)
    {
        var updatedCustomer = await _customerService.UpdateCustomerContactAsync(customerId, contactId, contactDto);
        if (updatedCustomer == null)
            return NotFound($"Customer with ID {customerId} or Contact with ID {contactId} not found.");

        return Ok(updatedCustomer);
    }

    /// <summary>
    /// Delete a specific contact from a customer.
    /// </summary>
    [HttpDelete("{customerId}/contacts/{contactId}")]
    public async Task<ActionResult> DeleteCustomerContact(int customerId, int contactId)
    {
        var isDeleted = await _customerService.DeleteCustomerContactAsync(customerId, contactId);
        if (!isDeleted)
            return NotFound($"Customer with ID {customerId} or Contact with ID {contactId} not found.");

        return NoContent();
    }
}
