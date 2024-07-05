using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using AutoMapper;
using HotelListingAPI.Contracts;

namespace HotelListingAPI.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesRepository countryRepository;
        private readonly IMapper mapper;

        public CountriesController(
            ICountriesRepository countryRepository,
            IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryResponseModel>>> GetCountries()
        {
            var countries =  await countryRepository.GetAllAsync();

            var response = this.mapper.Map<IEnumerable<GetCountryResponseModel>>(countries);

            return this.Ok(response);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCountryDetailsResponseModel>> GetCountry(int id)
        {
            var country = await countryRepository.GetDetails(id);

            if (country == null)
            {
                return NotFound();
            }

            var response = this.mapper.Map<GetCountryDetailsResponseModel>(country);
            return this.Ok(response);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryRequestModel updateCountryRequestModel)
        {
            if (id != updateCountryRequestModel.Id)
            {
                return BadRequest();
            }

            var country = await this.countryRepository.GetDetails(id);
            if (country == null)
            {
                return NotFound();
            }

            this.mapper.Map(updateCountryRequestModel, country);

            try
            {
                await this.countryRepository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryRequestModel createCountry)
        {

            var country = this.mapper.Map<Country>(createCountry);
            await this.countryRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await this.countryRepository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await this.countryRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await this.countryRepository.ExistsAsync(id);
        }
    }
}
