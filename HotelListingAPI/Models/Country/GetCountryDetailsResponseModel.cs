using HotelListingAPI.Data;
using HotelListingAPI.Models.Hotel;

namespace HotelListingAPI.Models.Country;

public class GetCountryDetailsResponseModel : BaseCountryModel
{
    public int Id { get; set; }

    public virtual IList<GetHotelDetailsResponseModel> Hotels { get; set; }
}
