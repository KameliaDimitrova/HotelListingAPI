using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using HotelListingAPI.Models.Hotel;
using HotelListingAPI.Models.Users;

namespace HotelListingAPI.Configurations;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        CreateMap<Country, GetCountryResponseModel>();
        CreateMap<Country, GetCountryDetailsResponseModel>();
        CreateMap<CreateCountryRequestModel, Country>();
        CreateMap<UpdateCountryRequestModel, Country>();

        CreateMap<Hotel, GetHotelResponseModel>();
        CreateMap<CreateHotelRequestModel, Hotel>();
        CreateMap<UpdateHotelRequestModel, Hotel>();

        CreateMap<CreateUserRequestModel, User>();
    }
}
