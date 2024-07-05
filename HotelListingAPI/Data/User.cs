using Microsoft.AspNetCore.Identity;

namespace HotelListingAPI.Data;

public class User : IdentityUser
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
