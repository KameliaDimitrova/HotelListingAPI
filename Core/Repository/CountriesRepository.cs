using AutoMapper;
using HotelListingAPI.Core.Contracts;
using HotelListingAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Core.Repository;

public class CountriesRepository(HotelListingDbContext context, IMapper mapper) : Repository<Country>(context, mapper), ICountriesRepository
{
    private readonly HotelListingDbContext context = context;

    public async Task<Country?> GetDetails(int id)
    {
        return await this.context.Countries.Include(q => q.Hotels)
            .FirstOrDefaultAsync(q=>q.Id == id);
    }
}
 