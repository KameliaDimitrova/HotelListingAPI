using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.Models.Country;

public abstract class BaseCountryModel
{
    public string Name { get; set; }

    [Required]
    public string ShortName { get; set; }
}
