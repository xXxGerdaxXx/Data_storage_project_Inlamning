using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorageAPI.Controllers
{
    [Route("api/currency")]
    [ApiController]
    public class CurrencyController(ICurrencyService currencyService) : ControllerBase
    {
        private readonly ICurrencyService _currencyService = currencyService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyDto>>> GetAllCurrencies()
        {
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            return Ok(currencies);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyDto>> GetCurrencyById(int id)
        {
            var currency = await _currencyService.GetCurrencyByIdAsync(id);
            if (currency == null)
                return NotFound($"Currency with ID {id} not found.");

            return Ok(currency);
        }

        [HttpPost]
        public async Task<ActionResult<CurrencyDto>> RegisterCurrency([FromBody] CurrencyRegistrationForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCurrency = await _currencyService.RegisterCurrencyAsync(form);
            if (createdCurrency == null)
                return BadRequest("Failed to create currency.");

            return CreatedAtAction(nameof(GetCurrencyById), new { id = createdCurrency.Id }, createdCurrency);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CurrencyDto>> UpdateCurrency(int id, [FromBody] CurrencyRegistrationForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedCurrency = await _currencyService.UpdateCurrencyAsync(id, form);
            if (updatedCurrency == null)
                return NotFound($"Currency with ID {id} not found.");

            return Ok(updatedCurrency);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCurrency(int id)
        {
            var isDeleted = await _currencyService.DeleteCurrencyAsync(id);
            if (!isDeleted)
                return NotFound($"Currency with ID {id} not found.");

            return NoContent();
        }
    }
}
