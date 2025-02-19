using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorageAPI.Controllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController(IStatusService statusService) : ControllerBase
    {
        private readonly IStatusService _statusService = statusService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusTypeDto>>> GetAllStatuses()
        {
            var statuses = await _statusService.GetAllStatusesAsync();
            return Ok(statuses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StatusTypeDto>> GetStatusById(int id)
        {
            var status = await _statusService.GetStatusByIdAsync(id);
            if (status == null)
                return NotFound($"Status with ID {id} not found.");

            return Ok(status);
        }

        [HttpPost]
        public async Task<ActionResult<StatusTypeDto>> RegisterStatus([FromBody] StatusRegistrationForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdStatus = await _statusService.RegisterStatusAsync(form);
            if (createdStatus == null)
                return BadRequest("Failed to create status.");

            return CreatedAtAction(nameof(GetStatusById), new { id = createdStatus.Id }, createdStatus);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StatusTypeDto>> UpdateStatus(int id, [FromBody] StatusRegistrationForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedStatus = await _statusService.UpdateStatusAsync(id, form);
            if (updatedStatus == null)
                return NotFound($"Status with ID {id} not found.");

            return Ok(updatedStatus);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStatus(int id)
        {
            var isDeleted = await _statusService.DeleteStatusAsync(id);
            if (!isDeleted)
                return NotFound($"Status with ID {id} not found.");

            return NoContent();
        }
    }
}
