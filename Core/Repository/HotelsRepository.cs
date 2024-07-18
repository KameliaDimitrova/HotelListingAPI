using AutoMapper;
using HotelListingAPI.Infrastructure;
using HotelListingAPI.Core.Contracts;

namespace HotelListingAPI.Core.Repository;

public class HotelsRepository(HotelListingDbContext context, IMapper mapper) : Repository<Hotel>(context, mapper), IHotelsRepository
{
}
