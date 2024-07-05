﻿using HotelListingAPI.Models.Hotel;

namespace HotelListingAPI.Models.Country;

public class GetCountryDetailsResponseModel : BaseCountryModel
{
    public int Id { get; set; }

    public virtual IList<GetHotelResponseModel> Hotels { get; set; }
}
