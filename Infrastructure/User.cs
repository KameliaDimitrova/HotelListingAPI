using Microsoft.AspNetCore.Identity;

namespace HotelListingAPI.Infrastructure;

public class User : IdentityUser
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
