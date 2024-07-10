using AutoMapper;
using HotelListingAPI.Contracts;
using HotelListingAPI.Data;

namespace HotelListingAPI.Repository;

public class HotelsRepository(HotelListingDbContext context, IMapper mapper) : Repository<Hotel>(context, mapper), IHotelsRepository
{
}
