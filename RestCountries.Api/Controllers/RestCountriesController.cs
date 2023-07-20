using Microsoft.AspNetCore.Mvc;
using RestCountries.Application.Interfaces;

namespace RestCountries.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("trade-route")]
        public async Task<IActionResult> GetTradeRoute([FromQuery] string countryA, [FromQuery] string countryB)
        {
            var route = await _countryService.GetTradeRoute(countryA, countryB);
            return Ok(route);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryService.GetAllCountries();
            return Ok(countries);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetCountryByName(string name)
        {
            var country = await _countryService.GetCountryByName(name);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetCountryByCode(string code)
        {
            var country = await _countryService.GetCountryByCode(code);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpGet("currency/{currency}")]
        public async Task<IActionResult> GetCountriesByCurrency(string currency)
        {
            var countries = await _countryService.GetCountriesByCurrency(currency);
            return Ok(countries);
        }
    }
}
