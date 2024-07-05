using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.Models.Country;

public abstract class BaseCountryModel
{
    public required string Name { get; set; }

    [Required]
    public required string ShortName { get; set; }
}
