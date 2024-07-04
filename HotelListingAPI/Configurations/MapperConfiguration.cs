using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using HotelListingAPI.Models.Hotel;

namespace HotelListingAPI.Configurations;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        CreateMap<CreateCountryRequestModel, Country>();
        CreateMap<Country, GetCountryResponseModel>();
        CreateMap<Country, GetCountryDetailsResponseModel>();
        CreateMap<Hotel, GetHotelDetailsResponseModel>();
        CreateMap<UpdateCountryRequestModel, Country>();
    }
}
