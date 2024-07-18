using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.Infrastructure;
using HotelListingAPI.Models.Country;
using AutoMapper;
using HotelListingAPI.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using HotelListingAPI.Exceptions;
using HotelListingAPI.Models;
using Microsoft.AspNetCore.OData.Query;

namespace HotelListingAPI.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController(
        ICountriesRepository countryRepository,
        IMapper mapper) : ControllerBase
    {
        private readonly ICountriesRepository countryRepository = countryRepository;
        private readonly IMapper mapper = mapper;

        // GET: api/Countries
        [HttpGet("all")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<GetCountryResponseModel>>> GetCountries()
        {
            var response =  await countryRepository.GetAllAsync<GetCountryResponseModel>();

            return this.Ok(response);
        }

        // GET: api/Countries/?StartIndex=0&pageSize=25&pageNumber=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryResponseModel>>> GetPagedCountries([FromQuery] QueryParametersRequestModel request)
        {
            var response = await countryRepository.GetAllAsync<GetCountryResponseModel>(request);

            return this.Ok(response);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCountryDetailsResponseModel>> GetCountry(int id)
        {
            var country = await countryRepository.GetDetails(id);

            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
            }

            var response = this.mapper.Map<GetCountryDetailsResponseModel>(country);
            return this.Ok(response);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryRequestModel updateCountryRequestModel)
        {
            if (id != updateCountryRequestModel.Id)
            {
                return BadRequest();
            }

            var country = await this.countryRepository.GetDetails(id);
            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
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
        [Authorize]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryRequestModel createCountry)
        {

            var country = this.mapper.Map<Country>(createCountry);
            await this.countryRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await this.countryRepository.GetAsync(id);
            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
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
