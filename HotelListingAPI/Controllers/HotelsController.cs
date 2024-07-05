using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.Data;
using HotelListingAPI.Contracts;
using AutoMapper;
using HotelListingAPI.Models.Hotel;

namespace HotelListingAPI.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelsController(
        IHotelsRepository hotelsRepository,
        IMapper mapper) : ControllerBase
    {
        private readonly IHotelsRepository hotelsRepository = hotelsRepository;
        private readonly IMapper mapper = mapper;

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetHotelResponseModel>>> GetHotels()
        {
            var hotels = await this.hotelsRepository.GetAllAsync();
            var result = this.mapper.Map<IEnumerable<GetHotelResponseModel>>(hotels);

            return this.Ok(result);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetHotelResponseModel>> GetHotel(int id)
        {
            var hotel = await this.hotelsRepository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            var result = this.mapper.Map<GetHotelResponseModel>(hotel);

            return this.Ok(result);
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelRequestModel request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            var hotel = await this.hotelsRepository.GetAsync(id);
            if(hotel == null)
            {
                return NotFound();
            }

            this.mapper.Map(request, hotel);
            try
            {
                await this.hotelsRepository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
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

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelRequestModel request)
        {
            var hotel = this.mapper.Map<Hotel>(request);
            await this.hotelsRepository.AddAsync(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await this.hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await this.hotelsRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await this.hotelsRepository.ExistsAsync(id);
        }
    }
}
