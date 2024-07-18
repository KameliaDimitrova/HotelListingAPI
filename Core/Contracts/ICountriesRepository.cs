using HotelListingAPI.Infrastructure;

namespace HotelListingAPI.Core.Contracts;

public interface ICountriesRepository : IRepository<Country>
{
    Task<Country?> GetDetails(int id);
}
