using Data_storage_project_library.Dtos;
using Data_storage_project_library.Helpers;
using Data_storage_project_library.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataStorageAPI.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController(ICustomerService customerService, ICustomerContactService customerContactService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;
    private readonly ICustomerContactService _customerContactService = customerContactService;

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
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, [FromBody] CustomerUpdateForm form)
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

    [HttpPost("{customerId}/contacts")]
    public async Task<ActionResult<CustomerDto>> AddCustomerContact(int customerId, [FromBody] CustomerContactDto contactDto)
    {
        if (!string.IsNullOrWhiteSpace(contactDto.Email) && !ValidationHelper.IsValidEmail(contactDto.Email))
            return BadRequest("Invalid email format.");

        if (!string.IsNullOrWhiteSpace(contactDto.Phone) && !ValidationHelper.IsValidPhoneNumber(contactDto.Phone))
            return BadRequest("Invalid phone number format.");

        var updatedCustomer = await _customerContactService.AddCustomerContactAsync(customerId, contactDto);
        if (updatedCustomer == null)
            return NotFound($"Customer with ID {customerId} not found.");

        return CreatedAtAction(nameof(GetCustomerById), new { id = updatedCustomer.Id }, updatedCustomer);
    }

    [HttpPut("{customerId}/contacts/{contactId}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomerContact(int customerId, int contactId, [FromBody] CustomerContactDto contactDto)
    {
        if (!string.IsNullOrWhiteSpace(contactDto.Email) && !ValidationHelper.IsValidEmail(contactDto.Email))
            return BadRequest("Invalid email format.");

        if (!string.IsNullOrWhiteSpace(contactDto.Phone) && !ValidationHelper.IsValidPhoneNumber(contactDto.Phone))
            return BadRequest("Invalid phone number format.");

        var updatedCustomer = await _customerContactService.UpdateCustomerContactAsync(customerId, contactId, contactDto);
        if (updatedCustomer == null)
            return NotFound($"Customer with ID {customerId} or Contact with ID {contactId} not found.");

        return Ok(updatedCustomer);
    }

    [HttpDelete("{customerId}/contacts/{contactId}")]
    public async Task<ActionResult> DeleteCustomerContact(int customerId, int contactId)
    {
        var isDeleted = await _customerContactService.DeleteCustomerContactAsync(customerId, contactId);
        if (!isDeleted)
            return NotFound($"Customer with ID {customerId} or Contact with ID {contactId} not found.");

        return NoContent();
    }
}
