using HotelListingAPI.Contracts;
using HotelListingAPI.Data;

namespace HotelListingAPI.Repository;

public class HotelsRepository(HotelListingDbContext context) : Repository<Hotel>(context), IHotelsRepository
{
}
