using HotelListingAPI.Data;

namespace HotelListingAPI.Contracts;

public interface ICountriesRepository : IRepository<Country>
{
    Task<Country?> GetDetails(int id);
}
