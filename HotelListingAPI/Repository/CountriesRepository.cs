using HotelListingAPI.Contracts;
using HotelListingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Repository;

public class CountriesRepository : Repository<Country>, ICountriesRepository
{
    private readonly HotelListingDbContext context;

    public CountriesRepository(HotelListingDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<Country?> GetDetails(int id)
    {
        return await this.context.Countries.Include(q => q.Hotels)
            .FirstOrDefaultAsync(q=>q.Id == id);
    }
}
 